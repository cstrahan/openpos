using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenPOS.Modules.Sales.ViewModels;

namespace OpenPOS.Modules.Sales.Views
{
    /// <summary>
    /// Interaction logic for CloseCashView.xaml
    /// </summary>
    public partial class CloseCashView_Main : UserControl
    {
        public CloseCashView_Main(CloseCashViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
