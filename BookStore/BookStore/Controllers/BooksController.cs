using BuisnessLayer.Interfaces;
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
    public class BooksController : ControllerBase
    {
        private readonly IBookBL bookBL;
        public BooksController(IBookBL bookBL)
        {
            this.bookBL = bookBL;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {

            try
            {
                var allbooks = bookBL.GetAllBooks();
                if (allbooks != null)
                {
                    return Ok(new { success = true, message = $"Show all Books", data = allbooks });
                }
                return Ok(new { success = true, message = "No books available" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

        }
    }
}
