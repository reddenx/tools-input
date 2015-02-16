﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Networking.Interfaces
{
    interface ISimpleDataSender
    {
        TransferInfo CurrentTransferInfo { get; }

        bool Connect(string host, int port);

        void StartDataTransfer(Stream inputStream);
    }
}
