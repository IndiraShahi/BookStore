using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public int BookId { get; set; }
        public bool OrderPlaced { get; set; }
        public DateTime OrderPlacedDate { get; set; }
    }
}
