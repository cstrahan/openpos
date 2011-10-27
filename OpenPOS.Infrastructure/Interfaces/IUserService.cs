using OpenPOS.Data.Models;

namespace OpenPOS.Infrastructure.Interfaces
{
    public interface IUserService
    {
        User[] GetUsers();

        User Login(string userName, string password, bool isPersistent, string customData);
        void Logout();

        User GetAuthenticatedUser();
    }
}
