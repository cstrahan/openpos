using System.Windows.Controls;
using OpenPOS.Data.Models;
using OpenPOS.Data.Storage;
using OpenPOS.Infrastructure.Editors;
using OpenPOS.Modules.Customers.ViewModels;

namespace OpenPOS.Modules.Customers.Views
{
    public partial class CustomersView_Administrator : UserControl
    {
        public CustomersView_Administrator(CustomersAdministrationViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
