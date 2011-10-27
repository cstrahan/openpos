using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenPOS.Data.Models;

namespace OpenPOS.Modules.Sales.Services
{
    public interface ICashupService
    {
        Payment[] GetPaymentsNotClosed();

        bool CloseCash();
    }
}
