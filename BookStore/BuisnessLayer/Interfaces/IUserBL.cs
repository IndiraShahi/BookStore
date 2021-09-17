using CommonLayer;
using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Interfaces
{
    public interface IUserBL
    {
        bool RegisterUser(User user);
        LoginResponse Login(string email, string password);
        bool ResetPassword(ResetPassword reset, int userId);
        string GenerateToken(string Email, int userId, string role);
        bool ForgetPassword(string email);
    }
}
