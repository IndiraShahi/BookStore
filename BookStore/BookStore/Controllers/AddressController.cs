using BuisnessLayer.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "users")]

    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {

        private IAddressBL _addressBL;
        public AddressController(IAddressBL addressBL)
        {
            this._addressBL = addressBL;
        }

        private int GetIdFromToken()
        {
            return Convert.ToInt32(User.FindFirst(x => x.Type == "userId").Value);
        }

        [HttpPost]
        public ActionResult AddNewAddress(Address address)
        {
            try
            {
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                int userId = Convert.ToInt32(principal.Claims.SingleOrDefault(c => c.Type == "UserId").Value);

                var newAddress = _addressBL.AddNewAddress(userId, address);

                if (newAddress == true)
                {
                    return Ok(new { success = true, message = "**Address added successfully**", data = address });
                }
                return BadRequest(new { success = false, message = "Please Fill Details properly!" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult GetAddresses()
        {
            try
            {
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                int userId = Convert.ToInt32(principal.Claims.SingleOrDefault(c => c.Type == "UserId").Value);
                var addresses = _addressBL.GetAllAddress(userId);

                if (addresses.Count != 0)
                {
                    return Ok(new { success = true, message = "Addresses are as follows : ", data = addresses });
                }
                return BadRequest(new { success = false, message = "There are no addresses" });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
