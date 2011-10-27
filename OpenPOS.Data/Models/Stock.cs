using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenPOS.Data.Models
{
    public class Stock
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public double Units { get; set; }
    }
}
