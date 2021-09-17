using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer
{
    public class AddressRequest
    {
        public long MobileNumber { get; set; }
        public string Addresses { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string typeOf { get; set; }
    }
}
