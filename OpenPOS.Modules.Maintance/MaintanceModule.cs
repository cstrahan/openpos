using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using OpenPOS.Infrastructure.Interfaces;
using OpenPOS.Infrastructure.Navigation;
using OpenPOS.Modules.Maintance.Views;

namespace OpenPOS.Modules.Maintance
{
    public class MaintanceModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        public MaintanceModule(IEventAggregator eventAggregator, INavigationService navigationService, IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
        }

        public void Navigate(NavigationPoint point)
        {
            var contentRegion = _regionManager.Regions["ContentRegion"];
            if (point.Name == "Maintance" && point.Category == "Administration")
            {
                var view = _container.Resolve<MaintanceView_Administrator>();
                contentRegion.Add(view);
                contentRegion.Activate(view);
            }
        }

        #region IModule Members

        public void Initialize()
        {
            _navigationService.RegisterNavigationPoint(
                new NavigationPoint()
                {
                    Name = "Maintance",
                    Category = "Administration",
                    ShowInJumpList = true,
                    Icon = "\\Images\\contents.png"
                });

            _eventAggregator.GetEvent<NavigateEvent>().Subscribe(Navigate, true);
        }

        #endregion
    }
}
