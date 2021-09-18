using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Interfaces
{
    public interface ICartBL
    {
        bool AddToCart(Cart cart, int userId);
    }
}
