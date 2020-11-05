﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        List<User> GetUsers();
        decimal TransferFunds(decimal amtToTransfer, Account sender, Account receiver, int userId);
        List<Transfer> GetListOfTransfers(int userId);
        Transfer GetDetailsOfTransfer(int transferId);
        List<Transfer> GetPendingTransfers(int userId);
    }
}
