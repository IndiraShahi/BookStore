using BuisnessLayer.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Services
{
    public class AddressBL : IAddressBL
    {
        private IAddressRL _addressRL;
        public AddressBL(IAddressRL addressRL)
        {
            this._addressRL = addressRL;
        }

        public bool AddNewAddress(int userId, Address address)
        {
            try
            {
                return this._addressRL.AddNewAddress(userId, address);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Address> GetAllAddress(int userId)
        {
            try
            {
                return this._addressRL.GetAllAddress(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
