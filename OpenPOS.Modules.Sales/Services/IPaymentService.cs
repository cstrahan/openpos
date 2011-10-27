using OpenPOS.ViewModels;
using OpenPOS.Data.Models;
namespace OpenPOS.Modules.Sales.Services
{
    public interface IPaymentService
    {
        bool ProcessPayments(TicketViewModel ticket, Payment[] payments);
    }
}