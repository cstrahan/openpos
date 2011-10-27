using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Composite.Presentation.Commands;
using OpenPOS.Data.Models;
using OpenPOS.Infrastructure.Browsers;
using OpenPOS.Infrastructure.Interfaces;
using System.Windows;
using System;
using System.Windows.Input;
using OpenPOS.Infrastructure.MVVM;
using OpenPOS.Modules.Stock.Services;

namespace OpenPOS.Modules.Stock.ViewModels
{
    public enum StockActionReason
    {
        Purchase,
        RefundIn,
        MovementIn,
        Sale,
        RefundOut,
        MovementOut,
        Break
    }

    public class StockMaintanceViewModel : ProductBrowserViewModel
    {
        private readonly IProductService _productService;
        private readonly IStockService _stockService;

        public StockMaintanceViewModel(IProductService productService, IStockService stockService)
        {
            _productService = productService;
            _stockService = stockService;

            Categories = new List<Category>(_productService.GetCategories());

            SelectedCategory = Categories.FirstOrDefault();

            Timestamp = DateTime.Now;
        }

        protected override void OnSelectedCategoryChanged(Category category)
        {
            base.OnSelectedCategoryChanged(category);

            Products = new ObservableCollection<Product>(_productService.GetProductsByCategory(category));
        }

        #region AddProductCommand

        private DelegateCommand<object> addProductCommand;
        public DelegateCommand<object> AddProductCommand
        {
            get
            {
                if (addProductCommand == null)
                {
                    addProductCommand = new DelegateCommand<object>((p) =>
                    {
                        var action = new StockActionViewModel()
                                        {
                                            Product = p as Product,
                                            Units = 1
                                        };

                        Actions.Add(action);

                        SelectedAction = action;
                    });
                }
                return addProductCommand;
            }
        }

        #endregion

        private StockActionViewModel _selectedAction;
        public StockActionViewModel SelectedAction
        {
            get { return _selectedAction; }   

            set
            {
                if (_selectedAction !=value)
                {
                    _selectedAction = value;
                    RaisePropertyChanged("SelectedAction");
                }
            }
        }

        ObservableCollection<StockActionViewModel> _actions = new ObservableCollection<StockActionViewModel>();
        public ObservableCollection<StockActionViewModel> Actions
        {
            get { return _actions; }
            set
            {
                if (_actions != value)
               {
                   _actions = value;
                   RaisePropertyChanged("Actions");
               }
            }
        }

        private DateTime _timestamp;
        public DateTime Timestamp
        {
            get { return _timestamp; } 
            set
            {
                if (_timestamp!=value)
                {
                    _timestamp = value;
                    RaisePropertyChanged("Timestamp");
                }
            }
        }  
        
        private StockActionReason _reason;
        public StockActionReason Reason
        {
            get { return _reason; } 
            set
            {
                if (_reason!=value)
                {
                    _reason = value;
                    RaisePropertyChanged("Reason");
                }
            }
        }

        #region ProcessTicket

        private ICommand _processActionsCommand;
        public ICommand ProcessActionsCommand
        {
            get
            {
                if (_processActionsCommand == null)
                {
                    _processActionsCommand = new RelayCommand(
                    (p) =>
                    {
                        foreach (var a in Actions)
                        {
                            StockAction action = new StockAction();
                            action.Id = Guid.NewGuid();
                            action.Price = a.Product.BuyPrice;
                            action.Timestamp = Timestamp;
                            if (Reason == StockActionReason.Purchase || Reason == StockActionReason.RefundIn || Reason == StockActionReason.MovementIn)
                                action.Units = a.Units;
                            else
                            {
                                action.Units = a.Units*-1;
                            }
                            action.ProductId = a.Product.Id;

                            // Add action to DB
                            _stockService.AddStockAction(action); 

                            // Update stock record
                            var stock = _stockService.GetStockByProductId(action.ProductId);
                            stock.Units += action.Units;

                            _stockService.UpdateStock(stock);
                        }
                        Actions.Clear();
                    },
                    (p) =>
                    {
                        return Actions.Count > 0;
                    });
                }
                return _processActionsCommand;
            }
        }

        #endregion

        #region IncreaseUnitsCommand

        private ICommand _increaseUnitsCommand;
        public ICommand IncreaseUnitsCommand
        {
            get
            {
                if (_increaseUnitsCommand == null)
                {
                    _increaseUnitsCommand = new RelayCommand(
                    (p) =>
                    {
                        SelectedAction.Units++;
                    },
                    (p) =>
                    {
                        return SelectedAction != null;
                    });
                }
                return _increaseUnitsCommand;
            }
        }

        #endregion

        #region DecreaseUnitsCommand

        private ICommand _decreaseUnitsCommand;
        public ICommand DecreaseUnitsCommand
        {
            get
            {
                if (_decreaseUnitsCommand == null)
                {
                    _decreaseUnitsCommand = new RelayCommand(
                    (p) =>
                    {
                        SelectedAction.Units--;
                        if (SelectedAction.Units==0)
                        {
                            Actions.Remove(SelectedAction);

                            SelectedAction = Actions.LastOrDefault();
                        }
                    },
                    (p) =>
                    {
                        return SelectedAction != null;
                    });
                }
                return _decreaseUnitsCommand;
            }
        }

        #endregion
    }
}
