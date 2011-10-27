using OpenPOS.ViewModels;

namespace OpenPOS.Modules.Sales.Services
{
    public interface ITicketService
    {
        TicketViewModel[] GetSharedTickets();
        void AddTicket(TicketViewModel ticket);
        void RemoveTicket(TicketViewModel ticket);

        void ProcessTicket(TicketViewModel ticket);

        OpenPOS.Data.Models.Payment[] GetPayments();
        OpenPOS.Data.Models.Ticket[] GetTickets();
    }
}
