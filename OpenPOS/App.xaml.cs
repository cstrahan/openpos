///////////////////////////////////////////////////////////////////////////////
// THIS CODE AND INFORMATION ARE PROVIdED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Windows;

using Microsoft.Practices.Composite.UnityExtensions;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using OpenPOS.Infrastructure.Behaviors;

namespace OpenPOS
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.AutoUpdate(true);

#if (DEBUG)
                RunInDebugMode();
#else
            RunInReleaseMode();
#endif

                this.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        private static void RunInDebugMode()
        {
            UnityBootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        private static void RunInReleaseMode()
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
            try
            {
                UnityBootstrapper bootstrapper = new Bootstrapper();
                bootstrapper.Run();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
        }

        private static void HandleException(Exception ex)
        {
            if (ex == null) return;

            ExceptionPolicy.HandleException(ex, "Default Policy");
            MessageBox.Show(OpenPOS.Properties.Resources.UnhandledException);
            Environment.Exit(1);
        }
    }
}
