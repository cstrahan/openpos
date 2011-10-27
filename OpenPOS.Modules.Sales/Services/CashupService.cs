using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenPOS.Data.Storage;
using OpenPOS.Data.Models;
using System.ComponentModel;
using System.Windows;
using OpenPOS.Infrastructure.Notifications;

namespace OpenPOS.Modules.Sales.Services
{
    public class CashupService : ICashupService
    {
        private readonly ISession _session;
        private readonly INotifyService _notifyService;

        public CashupService(ISession session, INotifyService notifyService)
        {
            _session = session;
            _notifyService = notifyService;
        }

        public Data.Models.Payment[] GetPaymentsNotClosed()
        {
            var payments = from p in _session.All<Payment>()
                           where p.CashupId.HasValue == false
                          select p;

            return payments.ToArray();
        }


        public bool CloseCash()
        {
            ClosedCash closedCash = new ClosedCash();
            closedCash.Id = Guid.NewGuid();
            closedCash.Timestamp = DateTime.Now;

            foreach (var payment in _session.All<Payment>())
            {
                payment.CashupId = closedCash.Id;
                _session.Update(payment);
                
            }

            _session.Add<ClosedCash>(closedCash);
            _session.CommitChanges();

            _notifyService.Notify("Sucessfuly closed cash\n" + closedCash.Id);

            return true;
        }
    }
}
