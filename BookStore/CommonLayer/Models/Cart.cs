using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}
