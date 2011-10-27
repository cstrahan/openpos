using System.Windows.Controls;
using OpenPOS.Modules.Navigation.ViewModels;

namespace OpenPOS.Modules.Navigation.Views
{
    public partial class NavigationView : UserControl
    {
        public NavigationView(NavigationViewModel viewModel)
        {
            InitializeComponent();

            // Set the ViewModel as this View's data context.
            this.DataContext = viewModel;
        }
    }
}
