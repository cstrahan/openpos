using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenPOS.Data.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Card { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Postal { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
        public string Notes { get; set; }
        public double Price { get; set; }
    }
}
