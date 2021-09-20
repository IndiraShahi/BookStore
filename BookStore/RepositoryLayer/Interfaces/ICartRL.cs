using CommonLayer;
using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface ICartRL
    {
        bool AddToCart(Cart cart, int userId);
        bool DeleteCart(CartRequest cart);

    }
}
