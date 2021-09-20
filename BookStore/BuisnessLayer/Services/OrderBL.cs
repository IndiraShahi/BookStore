using CommonLayer.Models;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Services
{
    public class OrderBL
    {
        private IOrderRL _orderRL;
        public OrderBL(IOrderRL orderRL)
        {
            this._orderRL = orderRL;
        }

        public bool EmailOrderDetails(int customerId, int orderNumber)
        {
            try
            {
                return this._orderRL.EmailOrderDetails(customerId, orderNumber);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool PlaceOrder(Order order, int UserId)
        {
            try
            {
                return this._orderRL.PlaceOrder(order, UserId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
