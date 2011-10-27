using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;
namespace OpenPOS.Infrastructure.Notifications
{
    public class NotifyService : INotifyService
    {
        TaskbarIcon _icon = new TaskbarIcon();
        private const string _title = "OpenPOS";

        public NotifyService()
        {
        }

        public void Notify(string message)
        {
            _icon.ShowBalloonTip(_title, message, BalloonIcon.Info);            
        }

        public bool Confirm(string message)
        {
            return MessageBox.Show(message, "OpenPOS", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }
    }
}