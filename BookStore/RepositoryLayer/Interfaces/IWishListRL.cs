using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IWishListRL
    {
        bool AddToWishlist(Wishlist wishlist, int UserId);
        bool RemoveFromWishlist(int wishlistId, int UserId);
    }
}
