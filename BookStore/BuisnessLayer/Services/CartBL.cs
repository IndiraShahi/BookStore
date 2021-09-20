using BuisnessLayer.Interfaces;
using CommonLayer;
using CommonLayer.Models;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Services
{
    public class CartBL : ICartBL
    {
        private ICartRL _cartRL;
        public CartBL(ICartRL cartRL)
        {
            this._cartRL = cartRL;
        }

        public bool AddToCart(Cart cart, int userId)
        {
            try
            {
                return this._cartRL.AddToCart(cart, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteCart(CartRequest cart)
        {
            return this._cartRL.DeleteCart(cart);
        }

    }
}
