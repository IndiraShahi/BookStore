using BuisnessLayer.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Services
{
    public class AdminBL : IAdminBL
    {
        private IAdminRL adminRL;
        public AdminBL(IAdminRL adminRL)
        {
            this.adminRL = adminRL;
        }

        public bool RegisterAdmin(Admin admin)
        {

            try
            {
                adminRL.RegisterAdmin(admin);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

