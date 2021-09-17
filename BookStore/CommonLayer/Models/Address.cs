using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Models
{
    public class Address
    {
        [Required]
        public int AddressId { get; set; }
        [Required]
        public string Addresses { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        public string TypeOf { get; set; }
        public int UserId { get; set; }
    }
}
