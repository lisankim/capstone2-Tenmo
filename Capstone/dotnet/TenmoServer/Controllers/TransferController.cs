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
        private static ITransferDAO TransferDAO;
        private static IUserDAO UserDAO;
        private static IAccountDAO AccountDAO;
        public TransferController(ITransferDAO _transferDAO, IUserDAO _userDAO, IAccountDAO _accountDAO)
        {
            TransferDAO = _transferDAO;
            UserDAO = _userDAO;
            AccountDAO = _accountDAO;
        }
        [HttpGet]
        public List<User> GetUsers()
        {
            return UserDAO.GetUsers();
        }
        [HttpPut]
        public decimal TransferFunds(Transfer t)
        {
            Account sender = AccountDAO.GetAccountById(t.AccountFrom);
            Account receiver = AccountDAO.GetAccountById(t.AccountTo);
            return TransferDAO.TransferFunds(t.Amount, sender, receiver);
        }

        [HttpGet("all")]
        public List<Transfer> GetTransfersList()
        {
            return TransferDAO.GetListOfTransfers();
        }
    }
}
