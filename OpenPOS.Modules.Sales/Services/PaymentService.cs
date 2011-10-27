using OpenPOS.Data.Models;
using OpenPOS.Infrastructure.Interfaces;
using OpenPOS.Data.Storage;
using System;
using System.Linq;

namespace OpenPOS.Modules.Sales.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUserService _userService;
        private readonly ISession _session;

        public PaymentService(IUserService userService, ISession session)
        {
            _userService = userService;
            _session = session;
        }

        public bool ProcessPayments(OpenPOS.ViewModels.TicketViewModel ticket, Data.Models.Payment[] payments)
        {
            Ticket t = new Ticket();
            t.Id = ticket.Id;
            t.UserId = _userService.GetAuthenticatedUser().Id;
            t.Metadata = ticket.Metadata;
            // Should find the Id from "some" settings file...
            t.TerminalId = _session.All<Terminal>().FirstOrDefault().Id;
            // Should find the Id from "some" settings file...
            //t.UserId = _session.All<User>().FirstOrDefault().Id;
            //t.CustomerId
            _session.Add<Ticket>(t);
            _session.CommitChanges();

            DateTime timestamp = DateTime.Now;

            foreach (var line in ticket.Lines)
            {
                TicketLine l = new TicketLine();
                l.Id = Guid.NewGuid();
                l.ProductId = line.Product.Id;
                l.Units = line.Units;
                l.Price = line.Price;

                l.TicketId = t.Id;

                _session.Add<TicketLine>(l);

                StockAction stockAction = new StockAction();
                stockAction.Id = Guid.NewGuid();
                stockAction.Timestamp = timestamp;
                stockAction.ProductId = line.Product.Id;
                stockAction.Price = line.Price;
                stockAction.Units = line.Units * -1;
                _session.Add<StockAction>(stockAction);
                _session.CommitChanges();

                var stock = _session.Single<Stock>(s => s.ProductId == line.Product.Id);
                if (stock != null)
                {
                    stock.Units -= line.Units;
                    _session.Update<Stock>(stock);
                    _session.CommitChanges();
                }
            }

            foreach (var payment in payments)
            {
                payment.TicketId = t.Id;
                payment.Timestamp = timestamp;
                payment.TerminalId = t.TerminalId;
                _session.Add<Payment>(payment);
                _session.CommitChanges();
            }

            return true;
        }
    }
}