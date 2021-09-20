using BuisnessLayer.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuisnessLayer.Services
{
    class WishlistBL : IWishlistBL
    {
        private IWishListRL _wishlistRL;
        public WishlistBL(WishListRL wishlistRL)
        {
            this._wishlistRL = wishlistRL;
        }
        public bool AddToWishlist(Wishlist wishlist, int UserId)
        {
            try
            {
                return this._wishlistRL.AddToWishlist(wishlist, UserId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool RemoveFromWishlist(int wishlistId, int UserId)
        {
            try
            {
                return this._wishlistRL.RemoveFromWishlist(wishlistId, UserId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
