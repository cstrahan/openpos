using System;
using System.Collections.Generic;
using System.Linq;
using OpenPOS.Data.Models;
using OpenPOS.Data.Storage;
using OpenPOS.Infrastructure.Interfaces;
using OpenPOS.Modules.Sales.ViewModels;
using OpenPOS.Modules.Sales.Views;
using OpenPOS.ViewModels;

namespace OpenPOS.Modules.Sales.Services
{
    public class TicketService : ITicketService
    {
        List<TicketViewModel> tickets = new List<TicketViewModel>();

        ISession _session;
        private readonly IUserService _userService;
        private readonly IPaymentService _paymentService;

        public TicketService(ISession session, IUserService userService, IPaymentService paymentService)
        {
            _session = session;
            _userService = userService;
            _paymentService = paymentService;
        }

        public TicketViewModel[] GetSharedTickets()
        {
            return tickets.ToArray();
        }

        public void AddTicket(TicketViewModel ticket)
        {
            tickets.Add(ticket);
        }

        public void RemoveTicket(TicketViewModel ticket)
        {
            tickets.Remove(ticket);
        }

        public void ProcessTicket(TicketViewModel ticket)
        {
            PaymentsViewModel viewModel = new PaymentsViewModel(ticket, _paymentService);
            PaymentsView view = new PaymentsView(viewModel);
            view.ShowDialog();

            RemoveTicket(ticket);
        }


        public Payment[] GetPayments()
        {
            return _session.All<Payment>().ToArray();
        }


        public Ticket[] GetTickets()
        {
            return _session.All<Ticket>().ToArray();
        }
    }
}
