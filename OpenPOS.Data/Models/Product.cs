using System;
using System.ComponentModel.DataAnnotations;

namespace OpenPOS.Data.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double BuyPrice { get; set; }
        public double SellPrice { get; set; }
        public Guid CategoryId { get; set; }
        public Guid TaxId { get; set; }
    }
}
