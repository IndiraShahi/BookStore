using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private IWishListRL _wishlistBL;
        public WishlistController(WishListRL wishlistBL)
        {
            this._wishlistBL = wishlistBL;
        }

        [HttpPost]
        public ActionResult AddToWishlist(Wishlist wishlist)
        {
            try
            {
                int UserId = Convert.ToInt32(User.FindFirst(x => x.Type == "userId").Value);

                var objectInWishlist = _wishlistBL.AddToWishlist(wishlist, UserId);

                if (objectInWishlist == true)
                {
                    return Ok(new { success = true, message = "**Added to wishlist successfully**", data = wishlist.BookId, wishlist.UserId });
                }
                return BadRequest(new { success = false, message = "Something is wrong!" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete]
        public ActionResult RemoveFromwishlist(int wishlistId)
        {
            try
            {
                int UserId = Convert.ToInt32(User.FindFirst(x => x.Type == "userId").Value);

                var bookToBeRemoved = _wishlistBL.RemoveFromWishlist(wishlistId, UserId);

                if (bookToBeRemoved == true)
                {
                    return Ok(new { message = "**Book removed from wishlist**" });
                }
                return BadRequest(new { message = "operation unsuccessfull -_-" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
