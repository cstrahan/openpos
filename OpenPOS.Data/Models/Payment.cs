using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenPOS.Data.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid TicketId { get; set; }
        public double Value { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid TerminalId { get; set; }
        public Nullable<Guid> CashupId { get; set; }
    }
}
