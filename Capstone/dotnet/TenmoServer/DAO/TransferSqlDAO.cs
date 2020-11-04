using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDAO : ITransferDAO
    {
        private readonly string connectionString;

        public TransferSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<User> GetUsers()
        {
            List<User> allUsers = new List<User>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM users", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        User u = ConvertReaderToUser(reader);
                        allUsers.Add(u);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return allUsers;
        }

        private User ConvertReaderToUser(SqlDataReader reader)
        {
            User u = new User();
            u.UserId = Convert.ToInt32(reader["user_id"]);
            u.Username = Convert.ToString(reader["username"]);
            return u;
        }
        public decimal TransferFunds(decimal amountToTransfer, Account sender, Account receiver)
        {
            TransferStatuses status = new TransferStatuses(2);
            if (sender.Balance >= amountToTransfer)
            {
                sender.Balance -= amountToTransfer;
                receiver.Balance += amountToTransfer;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand("UPDATE accounts SET balance = @newBalance WHERE user_id = @senderId; " +
                                                        "UPDATE accounts SET balance = @newBalance2 WHERE user_id = @receiverId; " +
                                                        "INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) VALUES (2, 2, @senderId, @receiverId, @amount)", conn);//hard coded type id
                        cmd.Parameters.AddWithValue("@newBalance", sender.Balance);
                        cmd.Parameters.AddWithValue("@newBalance2", receiver.Balance);
                        cmd.Parameters.AddWithValue("@senderId", sender.AccountId);
                        cmd.Parameters.AddWithValue("@receiverId", receiver.AccountId);
                        cmd.Parameters.AddWithValue("@amount", amountToTransfer);
                        SqlDataReader reader = cmd.ExecuteReader();
                        //while (reader.Read())
                        //{
                        //execute non query to get back rows affected to see if transfer successful
                        //}
                    }
                }
                catch (SqlException)
                {
                    status.TransferStatusId = 3;
                    throw;
                }
            }
            return sender.Balance;
        }

        public List<Transfer> GetListOfTransfers()
        {
            List<Transfer> allTransfer = new List<Transfer>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM transfers ", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Transfer t = ConvertReaderToTransfer(reader);
                        allTransfer.Add(t);
                    }
                }
            }
            catch
            {
                throw;
            }
            return allTransfer;
        }

        private Transfer ConvertReaderToTransfer(SqlDataReader reader)
        {
            Transfer t = new Transfer();
            t.TransferId = Convert.ToInt32(reader["transfer_id"]);
            t.TransferTypeId = Convert.ToInt32(reader["transfer_type_id"]);
            t.TransferStatusId = Convert.ToInt32(reader["transfer_status_id"]);
            t.AccountFrom = Convert.ToInt32(reader["account_from"]);
            t.AccountTo = Convert.ToInt32(reader["account_to"]);
            t.Amount = Convert.ToDecimal(reader["amount"]);
            return t;
        }
    }
}

