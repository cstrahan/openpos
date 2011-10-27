using System;
using System.Windows;
using System.Windows.Controls;
using OpenPOS.Data.Models;
using OpenPOS.Data.Storage;
using OpenPOS.Infrastructure.Editors;
using OpenPOS.Infrastructure.Interfaces;
using OpenPOS.Modules.Stock.ViewModels;
using OpenPOS.Modules.Stock.Services;
using OpenPOS.Infrastructure.Reports;
using CodeReason.Reports;
using System.Data;
using System.Windows.Xps.Packaging;
using System.IO;

namespace OpenPOS.Modules.Stock.Views
{
    public partial class StockView_Administrator : UserControl
    {
        public StockView_Administrator(StockAdministrationViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
