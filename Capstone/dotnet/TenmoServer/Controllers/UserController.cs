﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TenmoServer.DAO;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace TenmoServer.Controllers
{


    [Route("[controller]")]
    [ApiController]
    [Authorize]

    public class UserController : ControllerBase
    {

        private int? GetCurrentUserId()
        {
            string userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return null;
            int.TryParse(userId, out int userIdInt);
            return userIdInt;
        }

        private static IAccountDAO AccountDAO;

        public UserController(IAccountDAO _accountDAO)
        {
            AccountDAO = _accountDAO;
        }



        [HttpGet("{accountId}")]
        public decimal GetBalance()
           
        {
            int accountId = (int)GetCurrentUserId();

            return AccountDAO.GetBalance(accountId);
        }
    }
}
