using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer
{
    public class CartRequest
    {
        public int UserId { get; set; }
        public int CartId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
    }
}
