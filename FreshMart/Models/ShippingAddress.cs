using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreshMart.Models
{
    public class ShippingAddress
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public string HouseNo { get; set; }

        public string Street_Village { get; set; }

        public string CityArea_District { get; set; }

        public string PostalCode  { get; set; }
    }
}
