using System.Windows.Controls;
using OpenPOS.Modules.Sales.ViewModels;

namespace OpenPOS.Modules.Sales.Views
{
    public partial class SalesView_Main : UserControl
    {
        public SalesView_Main(SalesViewModel viewModel)
        {
            InitializeComponent();

            // Set the ViewModel as this View's data context.
            this.DataContext = viewModel;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }
    }
}
