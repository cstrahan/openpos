using System.Windows.Controls;
using OpenPOS.Data.Models;
using OpenPOS.Data.Storage;
using OpenPOS.Infrastructure.Editors;
using OpenPOS.Modules.Maintance.ViewModels;

namespace OpenPOS.Modules.Maintance.Views
{
    public partial class MaintanceView_Administrator : UserControl
    {
        public MaintanceView_Administrator(MaintanceAdministrationViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
