using System.Windows;
using OpenPOS.Modules.Sales.ViewModels;

namespace OpenPOS.Modules.Sales.Views
{
    public partial class PaymentsView : Window
    {
        public PaymentsView(PaymentsViewModel viewModel)
        {
            InitializeComponent();

            viewModel.RequestClose += new System.EventHandler(viewModel_RequestClose);
            
            DataContext = viewModel;
        }

        void viewModel_RequestClose(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
