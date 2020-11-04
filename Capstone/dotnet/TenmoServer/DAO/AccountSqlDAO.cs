using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Controllers;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDAO : IAccountDAO
    {
        private readonly string connectionString;

        public AccountSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }


        public decimal GetBalance(int accountId)
        {
            // Account account = null;
            UserController userController = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT balance FROM accounts WHERE account_id = @accountId", conn);
                    cmd.Parameters.AddWithValue("@accountId", accountId );
                    //SqlDataReader reader = cmd.ExecuteScalar();

                    return Convert.ToDecimal(cmd.ExecuteScalar());
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return 0;
        }

        
    }
}
