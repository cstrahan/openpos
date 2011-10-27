using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenPOS.Infrastructure.MVVM;
using OpenPOS.Data.Models;

namespace OpenPOS.Modules.Stock.ViewModels
{
    public class StockActionViewModel : ViewModelBase
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
    }
}
