
using System.Windows.Threading;
using System;
using System.Threading;
namespace OpenPOS.Infrastructure.MVVM
{
    public class ViewModelBase : ObservableObject
    {
        private bool isBusy = true;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    RaisePropertyChanged("IsBusy");
                }
            }
        }
        
        public ViewModelBase()
        {
            IsBusy = true;    

            Dispatcher.CurrentDispatcher.BeginInvoke((Action)delegate
                                                                 {
                                                                     Initialize();

                                                                     IsBusy = false;
                                                                 }, DispatcherPriority.SystemIdle);
        }

        protected  virtual void Initialize()
        {
        }
    }
}
