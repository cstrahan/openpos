using System;
using System.Collections.ObjectModel;
using OpenPOS.Data.Models;
using OpenPOS.Infrastructure.MVVM;

namespace OpenPOS.ViewModels
{
    public class TicketViewModel : ViewModelBase
    {
        private Guid id = Guid.NewGuid();
        public Guid Id 
        {
            get { return id; }

            set
            {
                if (id != value)
                {
                    id = value;
                    RaisePropertyChanged("Id");
                }
            }
        }

        public TicketViewModel()
        {
            //Id = new Guid();

            lines = new ObservableCollection<TicketLineViewModel>();
            lines.CollectionChanged += (s, e) =>
                {
                    Refresh();
                };
        }

        #region Lines

        private ObservableCollection<TicketLineViewModel> lines;
        public ObservableCollection<TicketLineViewModel> Lines 
        {
            get { return lines; }
            set
            {
                if (lines != value)
                {
                    lines = value;
                    RaisePropertyChanged("Lines");
                }
            }
        }

        #endregion

        #region Subtotal

        public double Subtotal
        {
            get
            {
                double subtotal = 0.0f;
                foreach (var line in lines)
                {
                    subtotal += (line.Price * line.Units);
                }

                return subtotal;
            }
        }

        #endregion

        #region Total

        public double Total
        {
            get
            {
                double total = 0.0f;
                foreach (var line in lines)
                {
                    total += (line.Price * line.Units) + (line.Price * line.Units * line.Tax);
                }
                return total;
            }
        }

        #endregion

        #region Taxes

        public double Taxes
        {
            get
            {
                double tax = 0.0f;
                foreach (var line in lines)
                {
                    tax += (line.Price * line.Tax * line.Units);
                }
                return tax;
            }
        }

        #endregion

        #region SelectedTicketLine

        private TicketLineViewModel selectedTicketLine;
        public TicketLineViewModel SelectedTicketLine
        {
            get { return selectedTicketLine; }
            set
            {
                if (selectedTicketLine != value)
                {
                    selectedTicketLine = value;
                    RaisePropertyChanged("SelectedTicketLine");
                }
            }
        }

        #endregion

        public void Refresh()
        {
            RaisePropertyChanged("Total");
            RaisePropertyChanged("Subtotal");
            RaisePropertyChanged("Taxes");
        }

        public string Metadata { get; set; }
    }

    public class TicketLineViewModel : ViewModelBase
    {
        #region Product

        private Product product = new Product();
        public Product Product
        {
            get { return product; }
            set
            {
                if (product != value)
                {
                    product = value;
                    RaisePropertyChanged("Product");
                }
            }
        }

        #endregion

        #region Tax

        private double tax;
        public double Tax
        {
            get { return tax; }
            set
            {
                if (tax != value)
                {
                    tax = value;
                    RaisePropertyChanged("Tax");
                }
            }
        }

        #endregion

        #region Units

        private double units;
        public double Units
        {
            get { return units; }
            set
            {
                if (units != value)
                {
                    units = value;
                    RaisePropertyChanged("Units");
                }
            }
        }

        #endregion

        #region Price

        private double price;
        public double Price
        {
            get { return price; }
            set
            {
                if (price != value)
                {
                    price = value;
                    RaisePropertyChanged("Price");
                }
            }
        }

        #endregion

        #region Value

        public double Value 
        {
            get { return Units * Price ; }
        }

        #endregion
    }
}
