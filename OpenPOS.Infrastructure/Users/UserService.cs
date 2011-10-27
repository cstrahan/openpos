using System.Linq;
using Microsoft.Practices.Composite.Events;
using OpenPOS.Data.Models;
using OpenPOS.Data.Storage;
using OpenPOS.Infrastructure.Interfaces;

namespace OpenPOS.Infrastructure.Users
{
    public class UserService : IUserService
    {
        private readonly ISession _session;
        private readonly IEventAggregator _eventAggregator;
        private User _currentUser;

        public UserService(ISession session, IEventAggregator eventAggregator)
        {
            _session = session;
            _eventAggregator = eventAggregator;
        }

        public User[] GetUsers()
        {
            return _session.All<User>().ToArray();
        }

        public User Login(string userName, string password, bool isPersistent, string customData)
        {
            // Check password and persist
            _currentUser = _session.Single<User>(u => u.Name == userName);

            _eventAggregator.GetEvent<LoginEvent>().Publish(_currentUser);

            return _currentUser;
        }

        public void Logout()
        {
            _currentUser = null;

            _eventAggregator.GetEvent<LogoutEvent>().Publish(_currentUser);
        }

        public User GetAuthenticatedUser()
        {
            return _currentUser;
        }
    }
}
