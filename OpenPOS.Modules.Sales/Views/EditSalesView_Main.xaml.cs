using System.Windows.Controls;
using OpenPOS.Modules.Sales.ViewModels;

namespace OpenPOS.Modules.Sales.Views
{
    public partial class EditSalesView_Main : UserControl
    {
        public EditSalesView_Main(EditSalesViewModel viewModel)
        {
            InitializeComponent();

            // Set the ViewModel as this View's data context.
            this.DataContext = viewModel;
        }
    }
}
