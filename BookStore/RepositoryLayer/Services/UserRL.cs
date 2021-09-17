using CommonLayer;
using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Interfaces;
using RepositoryLayer.MSMQServices;
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
        private readonly string secretKey;
        public UserRL(IConfiguration configuration)
        {
            connection = configuration.GetSection("ConnectionStrings").GetSection("OnlineBookStore").Value; 
            secretKey = configuration.GetSection("AppSettings").GetSection("Key").Value;
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
        private const string updateQuery = "spResetPassword";
        public bool ResetPassword(ResetPassword reset, int userId)
        {
            SqlConnection sqlconnection = new SqlConnection(connection);
            try
            {
                int rows;
                using (sqlconnection)
                {
                    sqlconnection.Open();
                    SqlCommand command = new SqlCommand(updateQuery, sqlconnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Password", reset.NewPassword);
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
                sqlconnection.Close();
            }
        }
        public string GenerateToken(string Email, int userId, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, Email),
                    new Claim("userId", userId.ToString(), ClaimValueTypes.Integer),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string jwtToken = tokenHandler.WriteToken(token);
            return jwtToken;
        }
        private const string _forgetPasswordQuery = "spForgetPassword";
        public bool ForgetPassword(string email)
        {
            SqlConnection sqlconnection = new SqlConnection(connection);
            try
            {
                using (sqlconnection)
                {
                    sqlconnection.Open();
                    SqlCommand command = new SqlCommand(_forgetPasswordQuery, sqlconnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@email", email);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        User user = new User();
                        while (reader.Read())
                        {
                            user.UserId = reader.GetInt32(0);
                            user.FullName = reader.GetString(1);
                            user.Email = reader.GetString(2);
                            user.Password = reader.GetString(3);
                            user.MobileNumber = reader.GetInt64(4);
                            user.Roles = reader.GetString(5);
                        }
                        if (user != null)
                        {
                            string token = GenerateToken(user.Email, user.UserId, user.Roles);
                            MSMQUtility mSMQ = new MSMQUtility();
                            mSMQ.SendMessage(email, token);

                            return true;
                        }
                        return false;
                    }
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sqlconnection.Close();
            }
        }
    }
}

