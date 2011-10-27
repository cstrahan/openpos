using OpenPOS.Infrastructure.MVVM;

namespace OpenPOS.Modules.Sales.ViewModels
{
    public class PaymentMethodViewModel : ViewModelBase

    {
        public string PaymentMethod { get; set; }

        private double _value = 0;
        public double Value 
        {
            get { return _value; }

            set
            {
                if (_value != value)
                {
                    _value = value;

                    RaisePropertyChanged("Value");
                }
            }
        }
    }
}
