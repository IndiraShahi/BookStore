using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Interfaces
{
    public interface IAddressBL
    {
        bool AddNewAddress(int userId, Address address);
        List<Address> GetAllAddress(int userId);
    }
}
