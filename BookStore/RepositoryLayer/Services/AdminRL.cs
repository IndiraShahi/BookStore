using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryLayer.Services
{
    public class AdminRL : IAdminRL
    {
        private readonly string connection;
        public AdminRL(IConfiguration configuration)
        {
            connection = configuration.GetSection("ConnectionStrings").GetSection("OnlineBookStore").Value;

        }
        private const string spQuery = "spAdminRegister";

        public bool RegisterAdmin(Admin admin)
        {
            SqlConnection sqlConnection = new SqlConnection(connection);
            try
            {
                int rows;
                using (sqlConnection)
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand(spQuery, sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@FullName", admin.FullName);
                    sqlCommand.Parameters.AddWithValue("@Email", admin.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", admin.Password);
                    sqlCommand.Parameters.AddWithValue("@MobileNumber", admin.MobileNumber);
                    rows = sqlCommand.ExecuteNonQuery();
                }
                return (rows > 0 ? true : false);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
        }
    }
}

