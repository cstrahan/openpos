namespace OpenPOS.Infrastructure.Notifications
{
    public interface INotifyService
    {
        void Notify(string message);
        bool Confirm(string message);
    }
}