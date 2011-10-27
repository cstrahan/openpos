using System.Collections.Generic;
///////////////////////////////////////////////////////////////////////////////
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
///////////////////////////////////////////////////////////////////////////////
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Composite.Presentation.Commands;
using OpenPOS.Data.Models;
using OpenPOS.Infrastructure.Interfaces;
using OpenPOS.Infrastructure.MVVM;

namespace OpenPOS.Modules.Users.ViewModels
{
    /// <summary>
    /// ViewModel for View1.
    /// </summary>
    public class UsersViewModel : INotifyPropertyChanged
    {
        private IUserService _userService;

        public UsersViewModel(IUserService userService)
        {
            _userService = userService;

            Users = new List<User>(_userService.GetUsers());

            ExitCommand = new DelegateCommand<object>(AppExit, CanAppExit);
        }

        private ICommand loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                if (loginCommand==null)
                {
                    loginCommand = new RelayCommand(
                        (u) =>
                            {
                                User user = (User) u;
                                _userService.Login(user.Name, string.Empty, true, string.Empty);
                            });
                }

                return loginCommand;
            } 
        }


        public List<User> Users { get; set; }

        #region ExitCommand
        public DelegateCommand<object> ExitCommand { get; private set; }

        private void AppExit(object commandArg)
        {
            Application.Current.Shutdown();
        }

        private bool CanAppExit(object commandArg)
        {
            return true;
        }
        #endregion

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
