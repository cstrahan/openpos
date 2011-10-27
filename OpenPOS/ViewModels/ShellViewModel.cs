///////////////////////////////////////////////////////////////////////////////
// THIS CODE AND INFORMATION ARE PROVIdED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
///////////////////////////////////////////////////////////////////////////////
using System.ComponentModel;
using System.Windows;
using Microsoft.Practices.Composite.Events;
using OpenPOS.Data.Models;
using OpenPOS.Infrastructure.Interfaces;
using OpenPOS.Infrastructure.Users;
using System;
using System.Deployment.Application;
using OpenPOS.Infrastructure.Navigation;
using OpenPOS.Infrastructure;
using OpenPOS.Properties;
using PixelLab.Wpf.Transitions;

namespace OpenPOS.ViewModels
{
    public class ShellViewModel : INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        private readonly INavigationService _navigationService;
        private readonly IEventAggregator _eventAggregator;

        private User currentUser;
        public User CurrentUser
        {
            get { return currentUser; }
            
            set
            {
                if (currentUser != value)
                {
                    currentUser = value;
                    NotifyPropertyChanged("CurrentUser");
                }
            }
        }

        private string version;
        public string Version
        {
            get { return version; }

            set
            {
                if (version != value)
                {
                    version = value;
                    NotifyPropertyChanged("Version");
                }
            }
        }

        private Visibility navigationPaneVisibility = Visibility.Collapsed;
        public Visibility NavigationPaneVisibility
        {
            get { return navigationPaneVisibility; }

            set
            {
                if (navigationPaneVisibility != value)
                {
                    navigationPaneVisibility = value;
                    NotifyPropertyChanged("NavigationPaneVisibility");
                }
            }
        }    


        public ShellViewModel(IUserService userService, INavigationService navigationService, IEventAggregator eventAggregator)
        {
            _userService = userService;
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;

            eventAggregator.GetEvent<LoginEvent>().Subscribe(
                (u) =>
                    {
                        CurrentUser = _userService.GetAuthenticatedUser();
                        NavigationPaneVisibility = Visibility.Visible;
                    },true);

            eventAggregator.GetEvent<LogoutEvent>().Subscribe(
                (u) =>
                {
                    CurrentUser = _userService.GetAuthenticatedUser();
                    NavigationPaneVisibility = Visibility.Collapsed;
                },true);

            SingleInstance.SingleInstanceActivated += new EventHandler<SingleInstanceEventArgs>(SingleInstance_SingleInstanceActivated);

            try
            {
                Version = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            catch (Exception ex)
            {
                Version = "Debug build";
            }
        }

        void SingleInstance_SingleInstanceActivated(object sender, SingleInstanceEventArgs e)
        {
            foreach (var arg in e.Args)
            {
                var point = _navigationService.GetNavigationPointFromArg(arg);
                if (point != null)
                {
                    _eventAggregator.GetEvent<NavigateEvent>().Publish(point);
                }
            }
            
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
