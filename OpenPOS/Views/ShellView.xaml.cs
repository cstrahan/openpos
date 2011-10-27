///////////////////////////////////////////////////////////////////////////////
// THIS CODE AND INFORMATION ARE PROVIdED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
///////////////////////////////////////////////////////////////////////////////
using System.Windows;

using OpenPOS.ViewModels;
using OpenPOS.Infrastructure.Interfaces;
using OpenPOS.Properties;
using PixelLab.Wpf.Transitions;

namespace OpenPOS.Views
{
    public partial class ShellView : Window
    {
        private readonly INavigationService _navigationService;

        public ShellView(ShellViewModel viewModel, INavigationService navigationService)
        {
            _navigationService = navigationService;
            InitializeComponent();

            // Set the ViewModel as this View's data context.
            this.DataContext = viewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Settings.Default.Save();
            _navigationService.ClearJumpList();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
            if (e.Key == System.Windows.Input.Key.F5)
            {
                if (WindowStyle == System.Windows.WindowStyle.None)
                {
                    WindowState = System.Windows.WindowState.Normal;
                    WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                }
                else
                {
                    WindowState = System.Windows.WindowState.Maximized;
                    WindowStyle = System.Windows.WindowStyle.None;
                }
            }
        }
    }
}
