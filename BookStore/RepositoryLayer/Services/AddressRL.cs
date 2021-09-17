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
    public class AddressRL : IAddressRL
    {
        private readonly string _connectionString;
        public AddressRL(IConfiguration config)
        {
            _connectionString = config.GetSection("ConnectionStrings").GetSection("OnlineBookStore").Value;
        }

        private const string _insertQuery = "spAddAddress";
        public bool AddNewAddress(int userId, Address address)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                int rows;
                using (connection)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(_insertQuery, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@address", address.Addresses);
                    command.Parameters.AddWithValue("@city", address.City);
                    command.Parameters.AddWithValue("@state", address.State);
                    command.Parameters.AddWithValue("@type_of", address.TypeOf);
                    command.Parameters.AddWithValue("@UserId", userId);
                    rows = command.ExecuteNonQuery();
                }
                return (rows > 0 ? true : false);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        private const string _selectQuery = "spGetAddresses";
        public List<Address> GetAllAddress(int userId)
        {
            List<Address> addresses = new List<Address>();
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(_selectQuery, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader sqlreader = command.ExecuteReader();
                while (sqlreader.Read())
                {
                    Address address1 = new Address();
                    address1.AddressId = Convert.ToInt32(sqlreader["address_id"]);
                    address1.Addresses = sqlreader["address"].ToString();
                    address1.City = sqlreader["city"].ToString();
                    address1.State = sqlreader["state"].ToString();
                    address1.TypeOf = sqlreader["type_of"].ToString();
                    addresses.Add(address1);
                }
                return addresses;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
