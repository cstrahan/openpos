using System.Windows;

namespace OpenPOS.Modules.Sales.Metadata
{
    public class SimpleMetadataPrompt : IMetadataProvider
    {
        public void Process(OpenPOS.ViewModels.TicketViewModel ticket)
        {
            if (ticket.Total > 200)
            {
                MessageBox.Show("Ask for phone/email");
            }
        }
    }
}
