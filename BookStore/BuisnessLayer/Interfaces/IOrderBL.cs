using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Interfaces
{
    public interface IOrderBL
    {
        bool PlaceOrder(Order order, int customerId);
        bool EmailOrderDetails(int UserId, int orderNumber);
    }
}
