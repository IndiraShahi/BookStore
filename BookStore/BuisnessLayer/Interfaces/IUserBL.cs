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
    }
}
