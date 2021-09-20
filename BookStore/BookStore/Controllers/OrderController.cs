using BuisnessLayer.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderBL _orderBL;
        public OrderController(IOrderBL orderBL)
        {
            this._orderBL = orderBL;
        }

        [HttpPost]
        public IActionResult PlaceOrder(Order order)
        {
            try
            {
                int customerId = Convert.ToInt32(User.FindFirst(x => x.Type == "userId").Value);

                var orderToBePlaced = this._orderBL.PlaceOrder(order, customerId);

                if (orderToBePlaced == true)
                {
                    _orderBL.EmailOrderDetails(customerId, order.OrderId);
                    return Ok(new { message = "**Order Placed Successfully**", data = order });
                }
                return BadRequest(new { success = false, message = "Something is wrong!" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
