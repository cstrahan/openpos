﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenPOS.Data.Models
{
    public class TicketLine
    {
        public Guid Id { get; set; }
        public double Units { get; set; }
        public double Price { get; set; }
        public Guid ProductId { get; set; }
        public Guid TicketId { get; set; }
    }
}
