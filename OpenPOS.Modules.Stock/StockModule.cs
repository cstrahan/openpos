using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using OpenPOS.Infrastructure.Interfaces;
using OpenPOS.Infrastructure.Navigation;
using OpenPOS.Modules.Stock.Views;
using OpenPOS.Modules.Stock.Services;

namespace OpenPOS.Modules.Stock
{
    public class StockModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        public StockModule(IEventAggregator eventAggregator, INavigationService navigationService, IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
        }

        public void Navigate(NavigationPoint point)
        {
            var contentRegion = _regionManager.Regions["ContentRegion"];
            if (point.Name == "Stock" && point.Category == "Administration")
            {
                var view = _container.Resolve<StockView_Administrator>();
                contentRegion.Add(view);
                contentRegion.Activate(view);
            }
        }

        #region IModule Members

        public void Initialize()
        {
            _container.RegisterType<IStockService, StockService>();

            _navigationService.RegisterNavigationPoint(
                new NavigationPoint()
                {
                    Name = "Stock",
                    Category = "Administration",
                    ShowInJumpList = true,
                    Icon = "\\Images\\contents.png"
                });

            _eventAggregator.GetEvent<NavigateEvent>().Subscribe(Navigate, true);
        }

        #endregion
    }
}
