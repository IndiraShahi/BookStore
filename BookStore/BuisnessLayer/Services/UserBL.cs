using BuisnessLayer.Interfaces;
using CommonLayer.Models;
using CommonLayer;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace BuisnessLayer.Services
{
    public class UserBL : IUserBL
    {
        private IUserRL userRL;
        private readonly string secretKey;
        public UserBL(IUserRL userRL, IConfiguration config)
        {
            this.userRL = userRL;
            secretKey = config.GetSection("AppSettings").GetSection("Key").Value;
        }

        public bool RegisterUser(User user)
        {

            try
            {
                userRL.RegisterUser(user);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public LoginResponse Login(string Email, string Password)
        {
            try
            {
                return this.userRL.Login(Email, Password);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool ResetPassword(ResetPassword reset, int userId)
        {
            try
            {
                return this.userRL.ResetPassword(reset, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool ForgetPassword(string email)
        {
            try
            {
                return this.userRL.ForgetPassword(email);
            }
            catch (Exception)
            {
                throw;
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
    }
}


