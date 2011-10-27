using System.Windows.Controls;
using OpenPOS.Modules.Stock.ViewModels;
using System;

namespace OpenPOS.Modules.Stock.Views
{
    /// <summary>
    /// Interaction logic for StockMaintanceView.xaml
    /// </summary>
    public partial class StockMaintanceView_Main : UserControl
    {
        public StockMaintanceView_Main()
        {
            InitializeComponent();

            reasonComboBox.ItemsSource = Enum.GetValues(typeof (StockActionReason));
        }
    }
}
