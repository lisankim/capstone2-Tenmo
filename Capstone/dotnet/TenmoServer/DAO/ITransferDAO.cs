using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        List<User> GetUsers();
        decimal TransferFunds(decimal amtToTransfer, Account sender, Account receiver);
        List<Transfer> GetListOfTransfers();
    }
}
