using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Microsoft.Practices.Composite.Events;
using OpenPOS.Infrastructure.Interfaces;
using OpenPOS.Infrastructure.MVVM;

namespace OpenPOS.Modules.Navigation.ViewModels
{
    public class NavigationViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly INavigationService _navigationService;

        private ObservableCollection<NavigationPointViewModel> navigationPoints = new ObservableCollection<NavigationPointViewModel>();
        public ObservableCollection<NavigationPointViewModel> NavigationPoints
        {
            get
            {
                return navigationPoints;
            }
            set
            {
                if (navigationPoints != value)
                {
                    navigationPoints = value;
                    RaisePropertyChanged("NavigationPoints");
                }
            }
        }

        public NavigationViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _navigationService = navigationService;

            foreach (var point in _navigationService.GetNavigationPoints())
            {
                navigationPoints.Add(new NavigationPointViewModel(_eventAggregator) 
                { 
                    Name = point.Name,
                    Category = point.Category,
                    Icon = point.Icon
                });
            }

            ICollectionView view = CollectionViewSource.GetDefaultView(NavigationPoints);
            if (view != null)
            {
                view.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            }

            _navigationService.CreateJumpList();
        }
    }
}
