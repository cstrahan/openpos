using Microsoft.Practices.Composite.Events;
///////////////////////////////////////////////////////////////////////////////
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
///////////////////////////////////////////////////////////////////////////////
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using OpenPOS.Infrastructure.Users;
using OpenPOS.Modules.Users.Views;

namespace OpenPOS.Modules.Users
{
    public class UsersModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;

        public UsersModule(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _container = container;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
        }

        private void DisplayUsersView()
        {
            var view = _container.Resolve<UsersView>();
            _regionManager.Regions["ContentRegion"].Add(view);
            _regionManager.Regions["ContentRegion"].Activate(view);
        }

        #region IModule Members

        public void Initialize()
        {
            DisplayUsersView();

            _eventAggregator.GetEvent<LogoutEvent>().Subscribe(
                (u) =>
                    {
                        DisplayUsersView();
                    }, true);

        }

        #endregion
    }
}
