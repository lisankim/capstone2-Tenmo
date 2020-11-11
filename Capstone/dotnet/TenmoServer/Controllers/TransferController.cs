using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TenmoServer.DAO;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TransferController :ControllerBase
    {
        private int? GetCurrentUserId()
        {
            string userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return null;
            int.TryParse(userId, out int userIdInt);
            return userIdInt;
        }
        private static ITransferDAO transferDAO;
        private static IUserDAO userDAO;
        private static IAccountDAO accountDAO;
        public TransferController(ITransferDAO _transferDAO, IUserDAO _userDAO, IAccountDAO _accountDAO)
        {
            transferDAO = _transferDAO;
            userDAO = _userDAO;
            accountDAO = _accountDAO;
        }
        [HttpGet]
        public List<User> GetUsers()
        {
            return userDAO.GetUsers();
        }
        [HttpPost]
        public decimal TransferFunds(Transfer t)
        {
            Account sender = accountDAO.GetAccountById(t.AccountFrom);
            Account receiver = accountDAO.GetAccountById(t.AccountTo);
            return transferDAO.TransferFunds(t.Amount, sender, receiver, (int)GetCurrentUserId());
        }

        [HttpGet("all")]
        public List<Transfer> GetTransfersList()
        {
            return transferDAO.GetListOfTransfers((int)GetCurrentUserId());
        }

        [HttpGet("{transferId}")]
        public Transfer GetDetailsOfTransfer(int transferId)
        {
            return transferDAO.GetDetailsOfTransfer(transferId); 
        }
        [HttpPost("pending")]
        public void RequestMoney(Transfer t)
        {
            t.AccountTo = (int)GetCurrentUserId();
            transferDAO.RequestMoney(t.Amount, accountDAO.GetAccountById(t.AccountFrom), accountDAO.GetAccountById(t.AccountTo), (int)GetCurrentUserId());
        }
        [HttpGet("pending")]
        public List<Transfer> GetPendingTransfers()
        {
            return transferDAO.GetPendingTransfers((int)GetCurrentUserId());
        }

        [HttpPut("pending")]
        public void ReceivePendingRequest(Transfer t)
        {
            transferDAO.ReceivePendingRequest(t.Amount, accountDAO.GetAccountById(t.AccountFrom), accountDAO.GetAccountById (t.AccountTo), (int)GetCurrentUserId(), t.TransferId);
        }

        [HttpPut("pending/rejected")]//might need to change url location since secondary put method
        public void RejectTransferRequest(Transfer t)
        {
            transferDAO.RejectTransferRequest(t);
        }
    }
}
