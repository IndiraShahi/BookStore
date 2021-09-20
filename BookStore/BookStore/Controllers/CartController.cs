using BuisnessLayer.Interfaces;
using CommonLayer;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "users")]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartBL _cartBL;
        public CartController(ICartBL cartBL)
        {
            this._cartBL = cartBL;
        }

        [HttpPost]
        public ActionResult AddToCart(Cart cart)
        {
            try
            {
                int userId = Convert.ToInt32(User.FindFirst(x => x.Type == "userId").Value);

                var objectInCart = _cartBL.AddToCart(cart, userId);

                if (objectInCart == true)
                {
                    return Ok(new { success = true, message = "**Added to cart successfully**", data = cart });
                }
                return BadRequest(new { success = false, message = "Something is wrong!" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
        [HttpDelete]
        public IActionResult DeleteCart(int cartId)
        {
            string message;
            try
            {
                int userId = Convert.ToInt32(User.FindFirst(x => x.Type == "userId").Value);
                CartRequest cart = new CartRequest
                {
                    CartId = cartId
                };
                bool result = _cartBL.DeleteCart(cart);
                if (result)
                {
                    message = "Successfully deleted cart details";
                    return this.Ok(new { message });
                }
                message = "Cart id is not match with our database.Please give correct bookId.";
                return BadRequest(new { message });
            }
            catch (Exception ex)
            {

                return this.BadRequest(new { Success = false, Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }
    }
}

