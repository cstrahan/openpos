using System;
using System.Windows.Input;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Commands;
using OpenPOS.Infrastructure.Navigation;

namespace OpenPOS.Modules.Navigation.ViewModels
{
    public class NavigationPointViewModel : NavigationPoint
    {
        IEventAggregator _eventAggregator;

        public NavigationPointViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        private ICommand navigateCommand;
        public ICommand NavigateCommand 
        {
            get
            {
                if (navigateCommand == null)
                {
                    navigateCommand = new DelegateCommand<NavigationPoint>(Navigate);
                }
                return navigateCommand;
            }
        }

        void Navigate(NavigationPoint point)
        {
            Console.WriteLine("Publish: " + point.Name + " [" + point.Category + "]");
            _eventAggregator.GetEvent<NavigateEvent>().Publish(point);
        }
    }
}
