using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenPOS.Infrastructure.MVVM;
using OpenPOS.Modules.Sales.Services;
using OpenPOS.Data.Models;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using OpenPOS.Infrastructure.Notifications;

namespace OpenPOS.Modules.Sales.ViewModels
{
    public class PaymentSummaryViewModel : ViewModelBase
    {
        public string Method { get; set; }
        public double Total { get; set; }
    }

    public class CloseCashViewModel : ViewModelBase
    {
        private readonly ICashupService _cashupService;
        private readonly INotifyService _notifyService;

        public CloseCashViewModel(ICashupService cashupService, INotifyService notifyService)
        {
            _cashupService = cashupService;
            _notifyService = notifyService;

            Payments = new ObservableCollection<Payment>(_cashupService.GetPaymentsNotClosed());
            Timestamp = DateTime.Now;
        }

        private DateTime timestamp;
        public DateTime Timestamp
        {
            get { return timestamp; }
            set
            {
                if (timestamp != value)
                {
                    timestamp = value;
                    RaisePropertyChanged("Timestamp");
                }
            }
        }

        private double total = 0;
        public double Total
        {
            get { return total; }
            set
            {
                if (total!=value)
                {
                    total = value;
                    RaisePropertyChanged("Total");
                }
            }
        }

        private double cash = 0;
        public double Cash
        {
            get { return cash; }
            set
            {
                if (cash != value)
                {
                    cash = value;
                    RaisePropertyChanged("Cash");
                }
            }
        }

        public DateTime Today
        {
            get { return DateTime.Now; }
        }

        public List<PaymentSummaryViewModel> PaymentSummary
        {
            get
            {
                List<PaymentSummaryViewModel> paymentSummaries = new List<PaymentSummaryViewModel>();

                var methods = (from m in Payments
                               select m.PaymentMethod).Distinct();

                foreach (var method in methods)
                {
                    paymentSummaries.Add(new PaymentSummaryViewModel()
                                             {
                                                 Method = method,
                                                 Total =
                                                     Payments.Where(m => m.PaymentMethod == method).Sum(m => m.Value)
                                             });    
                }

                return paymentSummaries;
            }
        }

        private ObservableCollection<Payment> payments;
        public ObservableCollection<Payment> Payments
        {
            get { return payments; }

            set
            {
                if (payments != value)
                {
                    payments = value;
                    RaisePropertyChanged("Payments");

                    
                    Cash = Payments.Where(p => p.PaymentMethod=="Cash").Sum(p => p.Value);
                    Total = Payments.Sum(p => p.Value);
                }
            }
        }

        #region CloseCash

        private ICommand closeCashCommand;
        public ICommand CloseCashCommand
        {
            get
            {
                if (closeCashCommand == null)
                {

                    closeCashCommand = new RelayCommand(
                    (p) =>
                    {
                        if (_notifyService.Confirm("Are you sure?"))
                        {
                            bool done = _cashupService.CloseCash();
                            if (done)
                            {
                                Total = 0;
                                Cash = 0;
                                
                                Payments.Clear();
                                RaisePropertyChanged("PaymentSummary");

                            }
                            else
                            {
                                _notifyService.Notify("Close cash failed!");
                            }
                        }
                    },
                    (p) =>
                    {
                        return Payments.Count > 0;
                    });
                }
                return closeCashCommand;
            }
        }

        #endregion

    }
}
