using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using OpenPOS.Infrastructure.Interfaces;
using OpenPOS.Infrastructure.Navigation;
using OpenPOS.Infrastructure.Users;
using OpenPOS.Modules.Sales.Services;
using OpenPOS.Modules.Sales.Views;

namespace OpenPOS.Modules.Sales
{
    public class SalesModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        public SalesModule(IEventAggregator eventAggregator, INavigationService navigationService, IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
        }

        public void Navigate(NavigationPoint point)
        {
            var contentRegion = _regionManager.Regions["ContentRegion"];
            if (point.Name == "Edit Sales")
            {
                var view = _container.Resolve<EditSalesView_Main>();
                contentRegion.Add(view);
                contentRegion.Activate(view);
            }
            if (point.Name == "Sales" && point.Category == "Main")
            {
                var view = _container.Resolve<SalesView_Main>();
                contentRegion.Add(view);
                contentRegion.Activate(view);
            }
            if (point.Name == "Sales" && point.Category == "Administration")
            {
                var view = _container.Resolve<SalesView_Administrator>();
                contentRegion.Add(view);
                contentRegion.Activate(view);
            }
            if (point.Name == "Close Cash" && point.Category == "Main")
            {
                var view = _container.Resolve<CloseCashView_Main>();
                contentRegion.Add(view);
                contentRegion.Activate(view);
            }
        }

        #region IModule Members

        public void Initialize()
        {
            _container.RegisterType<IProductService, ProductService>();
            _container.RegisterType<ICashupService, CashupService>();
            _container.RegisterType<IPaymentService, PaymentService>();
            
            _container.RegisterType<ITicketService, TicketService>(new ContainerControlledLifetimeManager());

            _eventAggregator.GetEvent<LoginEvent>().Subscribe(
                (u) =>
                    {
                        var view = _container.Resolve<SalesView_Main>();
                        _regionManager.Regions["ContentRegion"].Add(view);
                        _regionManager.Regions["ContentRegion"].Activate(view);
                    });

            _navigationService.RegisterNavigationPoint(
                new NavigationPoint() 
                { 
                    Name = "Sales",
                    Category = "Main",
                    ShowInJumpList = true,
                    Icon = "\\Images\\mycomputer.png"
                });
            _navigationService.RegisterNavigationPoint(
                new NavigationPoint() 
                { 
                    Name = "Edit Sales",
                    Category = "Main",
                    Icon = "/Images/mycomputer.png"
                });
            _navigationService.RegisterNavigationPoint(
                new NavigationPoint()
                {
                    Name = "Close Cash",
                    Category = "Main",
                    Icon = "/Images/mycomputer.png"
                });
            //_navigationService.RegisterNavigationPoint(
            //    new NavigationPoint()
            //    {
            //        Name = "Sales",
            //        Category = "Administration"
            //    });

            _eventAggregator.GetEvent<NavigateEvent>().Subscribe(Navigate, true);
        }

        #endregion
    }
}
