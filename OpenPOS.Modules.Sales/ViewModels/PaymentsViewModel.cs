using System;
using System.Collections.Generic;
using System.Linq;
using OpenPOS.Data.Models;
using OpenPOS.Infrastructure.MVVM;
using OpenPOS.ViewModels;
using System.Windows;
using OpenPOS.Modules.Sales.Services;

namespace OpenPOS.Modules.Sales.ViewModels
{
    public class PaymentsViewModel : DialogViewModel
    {
        TicketViewModel _ticket;
        private readonly IPaymentService _paymentService;

        public TicketViewModel Ticket
        {
            get { return _ticket; }
        }

        List<Payment> payments = new List<Payment>();

        public double Remaining
        {
            get
            {
                double payed = payments.Sum(p => p.Value);
                return Ticket.Total - payed;
            }
        }

        private List<PaymentMethodViewModel> methods = new List<PaymentMethodViewModel>();
        public List<PaymentMethodViewModel> Methods
        {
            get { return methods; }
        }

        private PaymentMethodViewModel selectedMethod;
        public PaymentMethodViewModel SelectedMethod
        {
            get { return selectedMethod; }
            set
            {
                if (selectedMethod != value)
                {
                    selectedMethod = value;
                    RaisePropertyChanged("SelectedMethod");
                }
            }
        }

        public PaymentsViewModel(TicketViewModel ticket, IPaymentService paymentService)
        {
            _ticket = ticket;
            _paymentService = paymentService;

            var defaultMethod = new PaymentMethodViewModel() { PaymentMethod = "Cash", Value = _ticket.Total };
            methods.Add(defaultMethod);
            methods.Add(new PaymentMethodViewModel() { PaymentMethod = "Cheque" });
            methods.Add(new PaymentMethodViewModel() { PaymentMethod = "Voucher" });
            methods.Add(new PaymentMethodViewModel() { PaymentMethod = "Card" });
            methods.Add(new PaymentMethodViewModel() { PaymentMethod = "Debt" });
            methods.Add(new PaymentMethodViewModel() { PaymentMethod = "Free" });
            SelectedMethod = defaultMethod;
        }

        #region AddPaymentCommand

        private RelayCommand addPaymentCommand;
        public RelayCommand AddPaymentCommand
        {
            get
            {
                if (addPaymentCommand == null)
                {
                    addPaymentCommand = new RelayCommand((p) =>
                    {
                        payments.Add(new Payment() { Id = Guid.NewGuid(), PaymentMethod = SelectedMethod.PaymentMethod, Value = SelectedMethod.Value});
                        RaisePropertyChanged("Remaining");

                        foreach (var method in Methods)
                        {
                            method.Value = 0;
                        }
                        var m = Methods.First();
                        m.Value = Remaining;
                        SelectedMethod = m;
                    },
                    (p) =>
                    {
                        if (SelectedMethod != null)
                        {
                            if (SelectedMethod.Value < Remaining)
                                return SelectedMethod.Value > 0;
                        }
                        return false;
                    });
                }
                return addPaymentCommand;
            }
        }

        #endregion

        #region ProcessCommand

        private RelayCommand processPaymentCommand;
        public RelayCommand ProcessPaymentCommand
        {
            get
            {
                if (processPaymentCommand == null)
                {
                    processPaymentCommand = new RelayCommand((p) =>
                    {
                        if (Remaining > 0)
                        {
                            payments.Add(new Payment()
                                             {
                                                 Id = Guid.NewGuid(),
                                                 PaymentMethod = SelectedMethod.PaymentMethod,
                                                 Value = SelectedMethod.Value
                                             });
                        }
                        
                        _paymentService.ProcessPayments(_ticket, payments.ToArray());

                        OnRequestClose();
                    },
                    (p) =>
                    {
                        if (SelectedMethod != null)
                            return SelectedMethod.Value >= Remaining;
                        return false;
                    });
                }
                return processPaymentCommand;
            }
        }

        #endregion
    }
}
