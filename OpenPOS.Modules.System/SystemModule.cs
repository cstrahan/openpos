using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using OpenPOS.Infrastructure.Interfaces;
using OpenPOS.Infrastructure.Navigation;

namespace OpenPOS.Modules.System
{
    public class SystemModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IUserService _userService;
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        public SystemModule(IEventAggregator eventAggregator, INavigationService navigationService, IUnityContainer container, IRegionManager regionManager, IUserService userService)
        {
            _container = container;
            _regionManager = regionManager;
            _userService = userService;
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
        }

        public void Navigate(NavigationPoint point)
        {
            if (point.Name == "Exit")
            {
                //Application.Current.Shutdown();
                _userService.Logout();
            }
        }

        #region IModule Members

        public void Initialize()
        {
            _navigationService.RegisterNavigationPoint(
                new NavigationPoint()
                {
                    Name = "Exit",
                    Category = "System",
                    ShowInJumpList = false,
                    Icon = "\\Images\\gohome.png"
                });

            _eventAggregator.GetEvent<NavigateEvent>().Subscribe(Navigate, true);
        }

        #endregion
    }
}
