using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IAddressRL
    {
        bool AddNewAddress(int userId, Address address);
        List<Address> GetAllAddress(int userId);
    }
}
