using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using OpenPOS.Data.Models;
using OpenPOS.Infrastructure.Browsers;
using OpenPOS.Infrastructure.Interfaces;
using OpenPOS.Infrastructure.MVVM;
using OpenPOS.Modules.Sales.Services;
using OpenPOS.ViewModels;
using System.Threading;
using OpenPOS.Infrastructure.Notifications;

namespace OpenPOS.Modules.Sales.ViewModels
{
    public class SalesViewModel : ProductBrowserViewModel
    {
        private readonly IProductService _productService;
        private readonly ITicketService _ticketService;
        private readonly IEventAggregator _eventAggregator;
        private readonly INotifyService _notifyService;

        public SalesViewModel(IProductService productService, ITicketService ticketService, IEventAggregator eventAggregator, INotifyService notifyService)
        {            
            _productService = productService;
            _ticketService = ticketService;
            _eventAggregator = eventAggregator;
            _notifyService = notifyService;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Categories = new List<Category>(_productService.GetCategories());

            SelectedCategory = Categories.FirstOrDefault();

            Tickets = new List<TicketViewModel>(_ticketService.GetSharedTickets());
            if (Tickets.Count > 0)
                SelectedTicket = Tickets.First();
        }

        #region BarcodeCommand

        private DelegateCommand<object> barcodeCommand;
        public DelegateCommand<object> BarcodeCommand
        {
            get
            {
                if (barcodeCommand == null)
                {
                    barcodeCommand = new DelegateCommand<object>((p) =>
                    {
                        TextBox barcodeTextBox = p as TextBox;
                        if (barcodeTextBox != null)
                        {
                            if (!string.IsNullOrEmpty(barcodeTextBox.Text))
                            {
                                string barcode = barcodeTextBox.Text;
                                Product product = _productService.GetProductByCode(barcode);
                                double tax = _productService.GetTaxByProduct(product).Rate;
                                //double tax = 0.00;
                                if (product!=null)
                                {
                                    if (SelectedTicket == null)
                                    {
                                        var ticket = new TicketViewModel();
                                        _ticketService.AddTicket(ticket);

                                        Tickets = new List<TicketViewModel>(_ticketService.GetSharedTickets());
                                        SelectedTicket = ticket;
                                    }
                                    SelectedTicket.Lines.Add(new TicketLineViewModel() { Product = (Product)product, Price = ((Product)product).SellPrice, Units = 1, Tax = tax });
                                }
                                else
                                {
                                    _notifyService.Notify("No product found");
                                }
                                barcodeTextBox.Text = string.Empty;
                            }
                        }
                    });
                }
                return barcodeCommand;
            }
        }

        #endregion
        
        #region AddTransaction

        private DelegateCommand<object> addTransactionCommand;
        public DelegateCommand<object> AddTransactionCommand
        {
            get
            {
                if (addTransactionCommand == null)
                {
                    addTransactionCommand = new DelegateCommand<object>((p) => 
                        {
                            Product product = (Product)p;
                            double tax = _productService.GetTaxByProduct(product).Rate;
                            if (SelectedTicket == null)
                            {
                                var ticket = new TicketViewModel();
                                _ticketService.AddTicket(ticket);

                                Tickets = new List<TicketViewModel>(_ticketService.GetSharedTickets());
                                SelectedTicket = ticket;
                            }
                            var line = new TicketLineViewModel() { Product = product, Price = product.SellPrice, Units = 1, Tax = tax };
                            SelectedTicket.Lines.Add(line);
                            SelectedTicket.SelectedTicketLine = line;
                        });
                }
                return addTransactionCommand;
            }
        }

        #endregion

        #region AddTicket

        private DelegateCommand<object> addTicketCommand;
        public DelegateCommand<object> AddTicketCommand
        {
            get
            {
                if (addTicketCommand == null)
                {
                    addTicketCommand = new DelegateCommand<object>((p) =>
                    {
                        var ticket = new TicketViewModel();
                        _ticketService.AddTicket(ticket);

                        Tickets = new List<TicketViewModel>(_ticketService.GetSharedTickets());
                        SelectedTicket = ticket;
                    });
                }
                return addTicketCommand;
            }
        }

        #endregion

        #region RemoveTicket

        private ICommand removeTicketCommand;
        public ICommand RemoveTicketCommand
        {
            get
            {
                if (removeTicketCommand == null)
                {

                    removeTicketCommand = new RelayCommand(
                    (p) =>
                    {
                        _ticketService.RemoveTicket(SelectedTicket);

                        Tickets = new List<TicketViewModel>(_ticketService.GetSharedTickets());
                        SelectedTicket = Tickets.FirstOrDefault();
                    },
                    (p) => 
                    {
                        return SelectedTicket != null;
                    });
                }
                return removeTicketCommand;
            }
        }

        #endregion

        #region ProcessTicket

        private ICommand processTicketCommand;
        public ICommand ProcessTicketCommand
        {
            get
            {
                if (processTicketCommand == null)
                {
                    processTicketCommand = new RelayCommand(
                    (p) =>
                    {
                        if (SelectedTicket != null)
                        {
                            _ticketService.ProcessTicket(SelectedTicket);

                            Tickets = new List<TicketViewModel>(_ticketService.GetSharedTickets());
                            SelectedTicket = Tickets.FirstOrDefault();
                        }
                    },
                    (p) =>
                    {
                        if (SelectedTicket != null)
                            return SelectedTicket.Lines.Count > 0;
                        return false;
                    });
                }
                return processTicketCommand;
            }
        }

        #endregion

        #region SelectedTicket

        private TicketViewModel selectedTicket;
        public TicketViewModel SelectedTicket 
        {
            get { return selectedTicket; }
            set
            {
                if (selectedTicket != value)
                {
                    selectedTicket = value;
                    RaisePropertyChanged("SelectedTicket");
                }
            }
        }

        #endregion

        #region Tickets

        private List<TicketViewModel> tickets = new List<TicketViewModel>();
        public List<TicketViewModel> Tickets
        {
            get { return tickets; }
            set
            {
                tickets = value;
                RaisePropertyChanged("Tickets");
            }
        }

        #endregion

        #region IncreaseUnitsCommand

        private ICommand increaseUnitsCommand;
        public ICommand IncreaseUnitsCommand
        {
            get
            {
                if (increaseUnitsCommand == null)
                {
                    increaseUnitsCommand = new RelayCommand(
                    (t) =>
                    {

                        if (SelectedTicket.SelectedTicketLine != null)
                        {
                            SelectedTicket.SelectedTicketLine.Units++;
                            SelectedTicket.Refresh();
                        }
                    },
                    (t) =>
                    {
                        if (SelectedTicket!=null)
                            return SelectedTicket.SelectedTicketLine != null;
                        return false;
                    });
                }
                return increaseUnitsCommand;
            }
        }

        #endregion

        #region DecreaseUnitsCommand

        private ICommand decreaseUnitsCommand;
        public ICommand DecreaseUnitsCommand
        {
            get
            {
                if (decreaseUnitsCommand == null)
                {
                    decreaseUnitsCommand = new RelayCommand((t) =>
                    {
                        if (SelectedTicket.SelectedTicketLine != null)
                        {
                            SelectedTicket.SelectedTicketLine.Units--;
                            if (SelectedTicket.SelectedTicketLine.Units == 0)
                            {
                                SelectedTicket.Lines.Remove(SelectedTicket.SelectedTicketLine);

                                SelectedTicket.SelectedTicketLine = SelectedTicket.Lines.LastOrDefault();

                                SelectedTicket.Refresh();
                            }
                        }
                    },
                    (t) =>
                    {
                        if (SelectedTicket != null)
                            return SelectedTicket.SelectedTicketLine != null;
                        return false;
                    });
                }
                return decreaseUnitsCommand;
            }
        }

        #endregion

        protected override void OnSelectedCategoryChanged(Category category)
        {
            base.OnSelectedCategoryChanged(category);

            Products = new ObservableCollection<Product>(_productService.GetProductsByCategory(category));
        }
    }
}
