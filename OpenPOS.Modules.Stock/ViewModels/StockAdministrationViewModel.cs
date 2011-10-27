using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenPOS.Infrastructure.Administration;
using OpenPOS.Data.Storage;
using OpenPOS.Infrastructure.Interfaces;
using OpenPOS.Infrastructure.MVVM;
using OpenPOS.Modules.Stock.Services;
using System.Windows;
using OpenPOS.Infrastructure.Editors;
using OpenPOS.Data.Models;
using CodeReason.Reports;
using System.IO;
using System.Data;
using System.Windows.Xps.Packaging;
using OpenPOS.Infrastructure.Reports;
using OpenPOS.Modules.Stock.Views;
using System.Windows.Threading;

namespace OpenPOS.Modules.Stock.ViewModels
{
    public class StockAdministrationViewModel : AdministrationViewModel
    {
        private readonly ISession _session;
        private readonly IProductService _productService;
        private readonly IStockService _stockService;

        public StockAdministrationViewModel(ISession session, IProductService productService, IStockService stockService)
        {
            _session = session;
            _productService = productService;
            _stockService = stockService;

            #region Maintance - Products

            Actions.Add(new AdministrationActionViewModel()
            {
                Action = (p) =>
                                {
                                    IsBusy = true;

                                    Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate
                                    {
                                        View = new EditorView(_session);
                                        var _viewModel = new EditorViewModel<Product>(_session);
                                        _viewModel.ItemRemoved += (sender, e) =>
                                            {
                                                _session.Delete<Data.Models.Stock>(s => s.ProductId == e.Item.Id);
                                                _session.CommitChanges();
                                            };
                                        _viewModel.NewItemSaved += (sender, e) =>
                                            {
                                                _session.Add<Data.Models.Stock>(new Data.Models.Stock()
                                                            {
                                                                Id = Guid.NewGuid(),
                                                                ProductId = e.Item.Id,
                                                                Units = 0
                                                            });
                                                _session.CommitChanges();
                                            };
                                        View.DataContext = _viewModel;

                                        IsBusy = false;
                                    }, DispatcherPriority.Background);
                                },
                Category = "Maintance",
                Title = "Products"
            });

            #endregion

            #region Maintance - Tax

            Actions.Add(new AdministrationActionViewModel()
            {
                Action = (p) =>
                {
                    IsBusy = true;
                    Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate
                                                                         {
                                                                             View = new EditorView(_session);
                                                                             View.DataContext =
                                                                                 new EditorViewModel<Tax>(_session);

                                                                             IsBusy = false;
                                    }, DispatcherPriority.Background);
                },
                Category = "Maintance",
                Title = "Tax"
            });

            #endregion

            #region Maintance - Categories

            Actions.Add(new AdministrationActionViewModel()
            {
                Action = (p) =>
                {
                    IsBusy = true;
                    Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate
                                                                         {
                                                                             View = new EditorView(_session);
                                                                             View.DataContext =
                                                                                 new EditorViewModel<Category>(_session);

                                                                             IsBusy = false;
                                    }, DispatcherPriority.Background);
                },
                Category = "Maintance",
                Title = "Categories"
            });

            #endregion

            #region Maintance - Stock Maintance

            Actions.Add(new AdministrationActionViewModel()
            {
                Action = (p) =>
                {
                    IsBusy = true;
                    Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate
                                                                         {

                                                                             View = new StockMaintanceView_Main();
                                                                             View.DataContext =
                                                                                 new StockMaintanceViewModel(
                                                                                     _productService, _stockService);

                                                                             IsBusy = false;
                                    }, DispatcherPriority.Background);
                },
                Category = "Maintance",
                Title = "Stock Maintance"
            });

            #endregion

            #region Reports - Products

            Actions.Add(new AdministrationActionViewModel()
            {
                Action = (p) =>
                {
                    IsBusy = true;
                    Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate
                                                                         {

                                                                             ReportDocument reportDocument =
                                                                                 new ReportDocument();

                                                                             StreamReader reader =
                                                                                 new StreamReader(
                                                                                     new FileStream(
                                                                                         @"Templates\ProductsReport.xaml",
                                                                                         FileMode.Open, FileAccess.Read));
                                                                             reportDocument.XamlData =
                                                                                 reader.ReadToEnd();
                                                                             reportDocument.XamlImagePath =
                                                                                 Path.Combine(
                                                                                     Environment.CurrentDirectory,
                                                                                     @"Templates\");
                                                                             reader.Close();


                                                                             ReportData reportData = new ReportData();
                                                                             DataTable table = new DataTable("Products");
                                                                             table.Columns.Add("Reference",
                                                                                               typeof (string));
                                                                             table.Columns.Add("Name", typeof (string));
                                                                             table.Columns.Add("BuyPrice",
                                                                                               typeof (double));
                                                                             table.Columns.Add("SellPrice",
                                                                                               typeof (double));

                                                                             foreach (
                                                                                 var product in
                                                                                     _productService.GetProducts())
                                                                             {
                                                                                 table.Rows.Add(new object[]
                                                                                                    {
                                                                                                        product.Reference,
                                                                                                        product.Name,
                                                                                                        product.BuyPrice,
                                                                                                        product.SellPrice
                                                                                                    });
                                                                             }
                                                                             reportData.DataTables.Add(table);

                                                                             XpsDocument xps =
                                                                                 reportDocument.CreateXpsDocument(
                                                                                     reportData);

                                                                             View = new SimpleReportView(xps);

                                                                             IsBusy = false;
                                                                         }, DispatcherPriority.SystemIdle);
                },
                Category = "Reports",
                Title = "Products"
            });

            #endregion

            #region Reports - Current Inventory

            Actions.Add(new AdministrationActionViewModel()
            {
                Action = (p) =>
                {
                    IsBusy = true;
                    Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate
                                                                         {
                                                                             ReportDocument reportDocument =
                                                                                 new ReportDocument();

                                                                             StreamReader reader =
                                                                                 new StreamReader(
                                                                                     new FileStream(
                                                                                         @"Templates\CurrentInventoryReport.xaml",
                                                                                         FileMode.Open, FileAccess.Read));
                                                                             reportDocument.XamlData =
                                                                                 reader.ReadToEnd();
                                                                             reportDocument.XamlImagePath =
                                                                                 Path.Combine(
                                                                                     Environment.CurrentDirectory,
                                                                                     @"Templates\");
                                                                             reader.Close();


                                                                             ReportData reportData = new ReportData();
                                                                             DataTable table = new DataTable("Products");
                                                                             table.Columns.Add("Reference",
                                                                                               typeof (string));
                                                                             table.Columns.Add("Name", typeof (string));
                                                                             table.Columns.Add("Units", typeof (double));

                                                                             foreach (
                                                                                 var product in
                                                                                     _productService.GetProducts())
                                                                             {
                                                                                 Data.Models.Stock stock =
                                                                                     _stockService.GetStockByProductId(
                                                                                         product.Id);
                                                                                 table.Rows.Add(new object[]
                                                                                                    {
                                                                                                        product.Reference,
                                                                                                        product.Name,
                                                                                                        stock.Units
                                                                                                    });
                                                                             }
                                                                             reportData.DataTables.Add(table);

                                                                             XpsDocument xps =
                                                                                 reportDocument.CreateXpsDocument(
                                                                                     reportData);

                                                                             View = new SimpleReportView(xps);

                                                                             IsBusy = false;
                                                                         }, DispatcherPriority.SystemIdle);
                },
                Category = "Reports",
                Title = "Current Inventory"
            });

            #endregion
        }
    }
}
        