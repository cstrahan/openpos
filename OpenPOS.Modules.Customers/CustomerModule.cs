using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using OpenPOS.Infrastructure.Interfaces;
using OpenPOS.Infrastructure.Navigation;
using OpenPOS.Modules.Customers.ViewModels;
using OpenPOS.Modules.Customers.Views;

namespace OpenPOS.Modules.Customers
{
    public class CustomerModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        public CustomerModule(IEventAggregator eventAggregator, INavigationService navigationService, IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
        }

        public void Navigate(NavigationPoint point)
        {
            var contentRegion = _regionManager.Regions["ContentRegion"];
            if (point.Name == "Customers" && point.Category == "Administration")
            {
                var view = _container.Resolve<CustomersView_Administrator>();
                contentRegion.Add(view);
                contentRegion.Activate(view);
            }
        }

        #region IModule Members

        public void Initialize()
        {
            //_navigationService.RegisterNavigationPoint(
            //    new NavigationPoint() 
            //    { 
            //        Name = "Customers",
            //        Category = "Main"
            //        ShowInJumpList = true
            //    });
            _navigationService.RegisterNavigationPoint(
                new NavigationPoint()
                {
                    Name = "Customers",
                    Category = "Administration",
                    ShowInJumpList = true,
                    Icon = "\\Images\\contents.png"
                });

            _eventAggregator.GetEvent<NavigateEvent>().Subscribe(Navigate, true);

        }

        #endregion
    }
}
