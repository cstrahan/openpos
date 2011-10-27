using OpenPOS.ViewModels;

namespace OpenPOS.Modules.Sales.Metadata
{
    public interface IMetadataProvider
    {
        void Process(TicketViewModel ticket);
    }
}
