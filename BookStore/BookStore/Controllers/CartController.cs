using BuisnessLayer.Interfaces;
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
    }
}

