using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRL : IUserRL
    {
        private readonly string connection;
        private readonly string secretkey;
        public UserRL(IConfiguration configuration)
        {
            connection = configuration.GetSection("ConnectionStrings").GetSection("OnlineBookStore").Value; 
            secretkey = configuration.GetSection("AppSettings").GetSection("Key").Value;
        }
        private const string spQuery = "spUserRegister";

        public bool RegisterUser(User user)
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
                    sqlCommand.Parameters.AddWithValue("@FullName", user.FullName);
                    sqlCommand.Parameters.AddWithValue("@Email", user.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", user.Password);
                    sqlCommand.Parameters.AddWithValue("@MobileNumber", user.MobileNumber);
                    rows = sqlCommand.ExecuteNonQuery();
                }
                return (rows > 0 ? true : false);
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        private const string _selectQuery = "spUserLogin";
        public LoginResponse Login(string Email, string Password)
        {
            SqlConnection sqlConnection = new SqlConnection(connection);
            try
            {
                using (sqlConnection)
                {
                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(_selectQuery, sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", Email);
                    command.Parameters.AddWithValue("@Password", Password);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        LoginResponse login = new LoginResponse();
                        while (reader.Read())
                        {
                            login.UserId = reader.GetInt32(0);
                            login.FullName = reader.GetString(1);
                            login.Email = reader.GetString(2);
                            login.MobileNumber = reader.GetInt64(3);
                            login.Roles = reader.GetString(4);
                        }
                        login.Token = GenerateToken(Email, login.UserId, login.Roles);
                        return login;
                    }
                    return null;
                }
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
        public string GenerateToken(string Email, int UserId, string Roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretkey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, Email),
                    new Claim("UserId", UserId.ToString(), ClaimValueTypes.Integer),
                    new Claim(ClaimTypes.Role, Roles)
                }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string jwtToken = tokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}

