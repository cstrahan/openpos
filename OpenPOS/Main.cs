using OpenPOS.Infrastructure;

namespace OpenPOS
{
    using System;
    using System.Reflection;

public static class OpenPOS_Main
{
    [STAThread]
    public static void Main()
    {
        if (SingleInstance.InitializeAsFirstInstance("OpenPOS"))
        {
            var application = new App();

            application.InitializeComponent();
            application.Run();

            // Allow single instance code to perform cleanup operations
            SingleInstance.Cleanup();
        }
    }
}
}