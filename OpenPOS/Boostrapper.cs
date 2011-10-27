///////////////////////////////////////////////////////////////////////////////
// THIS CODE AND INFORMATION ARE PROVIdED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
///////////////////////////////////////////////////////////////////////////////
using System.Windows;
using Microsoft.Practices.Composite.Logging;
using Microsoft.Practices.Composite.UnityExtensions;
using Microsoft.Practices.Unity;
using OpenPOS.Data.Storage;
using OpenPOS.Infrastructure.Interfaces;
using OpenPOS.Infrastructure.Navigation;
using OpenPOS.Infrastructure.Users;
using OpenPOS.Views;
using OpenPOS.Infrastructure.Notifications;
using Microsoft.Practices.Composite.Modularity;



namespace OpenPOS
{
    public partial class Bootstrapper : UnityBootstrapper
    {
        private readonly EnterpriseLibraryLogger _logger = new EnterpriseLibraryLogger();

        protected override void ConfigureContainer()
        {           
            RegisterTypeIfMissing<INavigationService, NavigationService>(true);
            RegisterTypeIfMissing<IUserService, UserService>(true);
            RegisterTypeIfMissing<INotifyService, NotifyService>(true);

            Container.RegisterInstance<ISession>(new SessionFactory().CreateSession("SQLCE"));

            base.ConfigureContainer();
        }

        protected override IModuleCatalog GetModuleCatalog()
        {
            var catalog = new ModuleCatalog();

            catalog.AddModule(typeof(OpenPOS.Modules.Sales.SalesModule), "NavigationModule", "UsersModule");
            catalog.AddModule(typeof(OpenPOS.Modules.Customers.CustomerModule), "NavigationModule", "UsersModule");
            catalog.AddModule(typeof(OpenPOS.Modules.Stock.StockModule), "NavigationModule", "UsersModule");
            catalog.AddModule(typeof(OpenPOS.Modules.System.SystemModule), "NavigationModule", "UsersModule");

            catalog.AddModule(typeof(OpenPOS.Modules.Navigation.NavigationModule));
            catalog.AddModule(typeof(OpenPOS.Modules.Users.UsersModule));

            return catalog;
        }

        protected override ILoggerFacade LoggerFacade
        {
            get { return _logger; }
        }

        protected override DependencyObject CreateShell()
        {
            ShellView view = Container.Resolve<ShellView>();

            view.Show();

            return view;
        }

        #region Bootstrapper Extensions

        void RegisterInstanceIfMissing<TFrom>(TFrom instance)
        {
            if (!Container.IsTypeRegistered(typeof(TFrom)))
            {
                Container.RegisterInstance<TFrom>(instance);
            }
        }

        void RegisterTypeIfMissing<TFrom, TTo>(bool registerAsSingleton) where TTo : TFrom
        {
            if (!Container.IsTypeRegistered(typeof(TFrom)))
            {
                if (registerAsSingleton)
                {
                    Container.RegisterType(typeof(TFrom), typeof(TTo), new ContainerControlledLifetimeManager());
                }
                else
                {
                    Container.RegisterType<TFrom, TTo>();
                }
            }
        }

        #endregion

    }
}

