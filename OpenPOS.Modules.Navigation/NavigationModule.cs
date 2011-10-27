using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using OpenPOS.Infrastructure.Interfaces;
using OpenPOS.Infrastructure.Users;
using OpenPOS.Modules.Navigation.Views;

namespace OpenPOS.Modules.Navigation
{
    public class NavigationModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        public NavigationModule(INavigationService navigationService , IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _container = container;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
        }

        #region IModule Members

        public void Initialize()
        {
            // Display the View in the Shell. Uses Prism's 'View
            // Discovery' mechanism to automatically display the view
            // in the specified named region.
            //_regionManager.RegisterViewWithRegion("NavigationRegion", () => _container.Resolve<NavigationView>());

            _eventAggregator.GetEvent<LoginEvent>().Subscribe(
                (u) =>
                    {
                        var view = _container.Resolve<NavigationView>();
                        _regionManager.Regions["NavigationRegion"].Add(view);
                        _regionManager.Regions["NavigationRegion"].Activate(view);          
                    }, true);
        }

        #endregion
    }
}
