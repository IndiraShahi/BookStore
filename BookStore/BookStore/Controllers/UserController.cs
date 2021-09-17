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
using System.Net;
using System.Security.Claims;
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
                    return Ok(new { success = true, message = "User Registered successfully", data = user.UserId,user.Email, user.FullName, user.Password,user.Roles });
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
        private int GetIdFromToken()
        {
            return Convert.ToInt32(User.FindFirst(x => x.Type == "userId").Value);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "users")]
        [HttpPut("resetpassword")]
        public ActionResult ResetPassword(ResetPassword reset)
        {
            try
            {
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                int userId = Convert.ToInt32(principal.Claims.SingleOrDefault(c => c.Type == "UserId").Value);

                var existingCustomer = userBL.ResetPassword(reset, userId);

                if (existingCustomer == true)
                {
                    return Ok(new { message = "Password reset successful" });
                }
                return BadRequest(new { success = false, message = "Password reset failed" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("forgetpassword")]
        public ActionResult ForgetPassword([FromBody] ForgetPassword fogetPassword)
        {
            try
            {
                var passwordForgottenUser = userBL.ForgetPassword(fogetPassword.Email);

                if (passwordForgottenUser == true)
                {
                    return Ok(new { message = "Link has been sent to given email id..." });
                }
                return BadRequest(new { success = false, message = "Invalid email address!" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
