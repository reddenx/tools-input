﻿using SMT.Networking.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace SMT.Networking.Udp
{
    public class UdpNetworkConnection<T> : IUdpNetworkConnection<T>
    {
        private const int DEFAULT_PORT = -1;

        public event EventHandler<T> OnMessageReceived;
        public event EventHandler<T> OnMessageSent;
        public event EventHandler<Exception> OnError;

        public string HostName { get; private set; }
        public int Port { get; private set; }

        private bool IsListening;

        private readonly UdpClient SendClient;
        private UdpClient ReceiveClient;

        private Thread SendThread;
        private Thread ReceiveThread;

        private readonly INetworkConnectionSerializer<T> Serializer;
        private readonly Queue<T> OutBox;

        public UdpNetworkConnection(INetworkConnectionSerializer<T> serializer)
        {
            OutBox = new Queue<T>();
            HostName = null;
            Port = DEFAULT_PORT;
            SendClient = new UdpClient();
            ReceiveClient = null;

            ReceiveThread = null;
            SendThread = null;

            Serializer = serializer;
        }

        //queues message in send queue
        public void Queue(T message)
        {
            OutBox.Enqueue(message);
        }

        //sends the send queue
        public void Send()
        {
            if (SendThread == null || !SendThread.IsAlive)
                SendThread = new Thread(SendLoop).StartBackground();
        }

        //queue then send all messages in queue
        public void Send(T message)
        {
            Queue(message);
            Send();
        }

        //unbind port, stop threads, kill event lists
        public void Dispose()
        {
            new Thread(() =>
            {
                ReceiveThread.DisposeOfThread();
                SendThread.DisposeOfThread();

                OnError.RemoveAllListeners();
                OnMessageReceived.RemoveAllListeners();
                OnMessageSent.RemoveAllListeners();

                try
                {
                    if (ReceiveClient != null && IsListening)
                        ReceiveClient.Close();
                }
                catch { }

            }).StartBackground();
        }

        //run once connected, cleanup once disconnected
        private void SendLoop()
        {
            try
            {
                while (OutBox.Count > 0)
                {
                    var message = OutBox.Dequeue();
                    byte[] buffer = null;
                    try
                    {
                        buffer = Serializer.Serialize(message);
                    }
                    catch (Exception e)
                    {
                        OnError.SafeExecute(this, e);
                    }

                    if (buffer != null && !string.IsNullOrWhiteSpace(HostName) && Port > 0)
                    {
                        SendClient.Send(buffer, buffer.Length, HostName, Port);
                        OnMessageSent.SafeExecute(this, message);
                    }
                }
            }
            catch (IOException e)
            {
                OnError.SafeExecute(this, e);
            }
            catch (ThreadAbortException aborted) { } //expected abort procedure given blocking call
            finally
            {
                //cleanup
            }
        }

        private void ReceiveLoop()
        {
            try
            {
                while (IsListening)
                {
                    var remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
                    var data = ReceiveClient.Receive(ref remoteEndpoint);

                    try
                    {
                        var message = Serializer.Deserialize(data);
                        OnMessageReceived.SafeExecute(this, message);
                    }
                    catch (Exception e)
                    {
                        OnError.SafeExecute(this, e);
                    }
                }
            }
            catch (IOException e)
            {
                StopListening();
                OnError.SafeExecute(this, e);
            }
            catch (ThreadAbortException) { }//expecting these on abort
        }

        //bind to local port
        public bool StartListening(int port)
        {
            if (IsListening)
            {
                OnError.SafeExecute(this, new IOException("udp networkconnection is already listening"));
                return false;
            }

            IsListening = true;
            CleanupListener();
            ReceiveThread.DisposeOfThread();

            try
            {
                ReceiveClient = new UdpClient(port);
                ReceiveThread = new Thread(ReceiveLoop).StartBackground();
                Port = port;
                IsListening = true;
                return true;
            }
            catch (IOException e)
            {
                OnError.SafeExecute(this, e);
                CleanupListener();
                StopListening();
                return false;
            }
        }

        //unbind local port
        public void StopListening()
        {
            if (!IsListening)
                return;

            IsListening = false;
            ReceiveThread.DisposeOfThread();
            CleanupListener();
        }

        private void CleanupListener()
        {
            if (ReceiveClient != null)
            {
                try
                {
                    ReceiveClient.Close();
                }
                catch (IOException e)
                {
                    OnError.SafeExecute(this, e);
                }
                ReceiveClient = null;
            }
        }

        //direct output towards endpoint
        public void Target(string hostname, int port)
        {
            HostName = hostname;
            Port = port;
        }

        //direct output towards endpoint
        public void Target(string connectionString)
        {
            var pieces = connectionString?.Split(':');
            if (pieces?.Length != 2)
                throw new ArgumentException("Improperly formatted string, expecting NAME:PORT e.g. www.oodlesofboodlesnoodles.com:9000");

            int port = -1;
            if (!int.TryParse(pieces[1], out port))
                throw new ArgumentException("Improperly formatted string, expecting NAME:PORT e.g. www.oodlesofboodlesnoodles.com:9000");

            Port = port;
            HostName = pieces[0];
        }

        //stop listening to incoming messages, unbind port
        public void Untarget()
        {
            HostName = null;
            Port = DEFAULT_PORT;
        }
    }
}
