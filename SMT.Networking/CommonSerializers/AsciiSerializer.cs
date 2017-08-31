﻿using SMT.Networking.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Networking.CommonSerializers
{
    public class AsciiSerializer : INetworkConnectionSerializer<string>
    {
        public byte[] Serialize(string message)
        {
            return Encoding.ASCII.GetBytes(message);
        }

        public string Deserialize(byte[] data)
        {
            return Encoding.ASCII.GetString(data);
        }
    }
}
