using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IOrderRL
    {
        bool PlaceOrder(Order order, int UserId);
        bool EmailOrderDetails(int UserId, int orderNumber);
    }
}
