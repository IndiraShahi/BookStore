using CommonLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Interfaces
{
    interface IWishlistBL
    {
        bool AddToWishlist(Wishlist wishlist, int UserId);
        bool RemoveFromWishlist(int wishlistId, int UserId);
    }
}
