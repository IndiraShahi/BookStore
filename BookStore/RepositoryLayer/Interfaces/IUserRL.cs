using CommonLayer;
using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRL
    {
        bool RegisterUser(User user);
        LoginResponse Login(string Email, string Password);
        bool ResetPassword(ResetPassword reset, int userId);
        bool ForgetPassword(string email);
    }
}
