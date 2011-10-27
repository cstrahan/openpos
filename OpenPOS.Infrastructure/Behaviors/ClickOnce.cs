using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Deployment.Application;
using System.Runtime.InteropServices;

namespace OpenPOS.Infrastructure.Behaviors
{
    public static class ClickOnce
    {
        private static int timerDuration = 30000;
        private static bool promptToRestartOnUpdate = true;

        private static System.Timers.Timer updateTimer = new System.Timers.Timer(timerDuration);

        public static void AutoUpdate(this Application app, bool autoUpdate)
        {
            if (autoUpdate == true)
            {
                updateTimer.Elapsed += new System.Timers.ElapsedEventHandler(updateTimer_Elapsed);
                updateTimer.AutoReset = false;
                updateTimer.Start();
            }
        }

        private static void updateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (HasUpdate())
            {
                Update();
            }
            else
            {
                updateTimer.Start();
            }
        }

        private static bool HasUpdate()
        {
            UpdateCheckInfo info = null;

            if (ApplicationDeployment.IsNetworkDeployed)
            {               

                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = ad.CheckForDetailedUpdate();
                }
                catch (DeploymentDownloadException dde)
                {
                    Console.WriteLine("The new version of the application cannot be downloaded at this time. Error: " + dde.Message);
                    return false;
                }
                catch (InvalidDeploymentException ide)
                {
                    Console.WriteLine("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Error: " + ide.Message);
                    return false;
                }
                catch (InvalidOperationException ioe)
                {
                    Console.WriteLine("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
                    return false;
                }

                if (info != null)
                {
                    return info.UpdateAvailable;
                }
            }

            return false;
        }

        private static void InitiateRestart(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            System.Windows.MessageBoxResult result = System.Windows.MessageBoxResult.No;
            if (promptToRestartOnUpdate)
                result = System.Windows.MessageBox.Show("An update has been installed. Would you like to restart the application?", "Update Installed", System.Windows.MessageBoxButton.YesNo);

            if (result == System.Windows.MessageBoxResult.Yes) System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(Restart));
        }

        private static void Update()
        {
            ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
            ad.UpdateCompleted += new System.ComponentModel.AsyncCompletedEventHandler(InitiateRestart);

            try
            {
                ad.UpdateAsync();
            }
            catch (DeploymentDownloadException dde)
            {
                Console.WriteLine("Cannot install the latest version of the application. Error: " + dde);
            }
        }

        private static void Restart()
        {
            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }
    }
}
