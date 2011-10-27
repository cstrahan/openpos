using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenPOS.Data.Models
{
    public class Ticket    
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Nullable<Guid> CustomerId { get; set; }
        public string Metadata { get; set; }
        public Guid TerminalId { get; set; }
    }
}
