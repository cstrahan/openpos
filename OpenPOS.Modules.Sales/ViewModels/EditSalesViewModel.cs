using System.Collections.Generic;
using Microsoft.Practices.Composite.Events;
using OpenPOS.Data.Models;
using OpenPOS.Infrastructure.MVVM;
using OpenPOS.Modules.Sales.Services;

namespace OpenPOS.Modules.Sales.ViewModels
{
    public class EditSalesViewModel : ViewModelBase
    {
        private readonly ITicketService _ticketService;
        private readonly IEventAggregator _eventAggregator;

        public EditSalesViewModel(ITicketService ticketService, IEventAggregator eventAggregator)
        {
            _ticketService = ticketService;
            _eventAggregator = eventAggregator;

            Tickets = new List<Ticket>(ticketService.GetTickets());
        }

        private List<Ticket> tickets;
        public List<Ticket> Tickets
        {
            get { return tickets; }
            set
            {
                if (tickets != value)
                {
                    tickets = value;
                    RaisePropertyChanged("Tickets");
                }
            }
        }
    }
}
