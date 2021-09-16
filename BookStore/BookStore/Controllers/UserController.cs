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
    public class UserController : ControllerBase
    {
        private IUserBL userBL;
        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }

        [HttpPost("Register")]
        public ActionResult RegisterUser(User user)
        {
            try
            {
                var existingCustomer = userBL.RegisterUser(user);

                if (user != null)

                {
                    return Ok(new { success = true, message = "User Registered successfully", data = user.Email, user.FullName, user.Password });
                }
                return Ok(new { Success = false, Message = "Registration Failed, User Already Exists" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

        }
        [HttpPost("login")]
        public ActionResult Userlogin(Login login)
        {
            try
            {
                var existingCustomer = userBL.Login(login.Email, login.Password);

                if (existingCustomer != null)
                {
                    return Ok(new { success = true, message = "**Login successfull**", data = existingCustomer });
                }
                return BadRequest(new { success = false, message = "Login Failed! Please try again..." });
            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

    }
}
