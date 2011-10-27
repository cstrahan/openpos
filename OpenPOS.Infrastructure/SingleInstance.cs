namespace OpenPOS.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Channels.Ipc;
    using System.Runtime.Serialization.Formatters;
    using System.Threading;
    using System.Windows;
    //using Standard;

    public class SingleInstanceEventArgs : EventArgs
    {
        public IList<string> Args { get; internal set; }
    }

    public static class SingleInstance
    {
        public static event EventHandler<SingleInstanceEventArgs> SingleInstanceActivated;

        private class _IpcRemoteService : MarshalByRefObject
        {
            /// <summary>Activate the first instance of the application.</summary>
            /// <param name="args">Command line arguemnts to proxy.</param>
            public void InvokeFirstInstance(IList<string> args)
            {
                if (Application.Current != null && !Application.Current.Dispatcher.HasShutdownStarted)
                {
                    Application.Current.Dispatcher.BeginInvoke((Action<object>)((arg) => SingleInstance._ActivateFirstInstance((IList<string>)arg)), args);
                }
            }

            /// <summary>Overrides the default lease lifetime of 5 minutes so that it never expires.</summary>
            public override object InitializeLifetimeService()
            {
                return null;
            }
        }

        private const string _RemoteServiceName = "SingleInstanceApplicationService";
        private static Mutex _singleInstanceMutex;
        private static IpcServerChannel _channel;

        public static bool InitializeAsFirstInstance(string applicationName)
        {
            IList<string> commandLineArgs = Environment.GetCommandLineArgs() ?? new string[0];

            // Build a repeatable machine unique name for the channel.
            string appId = applicationName + Environment.UserName;
            string channelName = appId + ":SingleInstanceIPCChannel";

            bool isFirstInstance;
            _singleInstanceMutex = new Mutex(true, appId, out isFirstInstance);
            if (isFirstInstance)
            {
                _CreateRemoteService(channelName);
            }
            else
            {
                _SignalFirstInstance(channelName, commandLineArgs);
            }

            return isFirstInstance;
        }

        public static void Cleanup()
        {
            _singleInstanceMutex.Dispose();

            if (_channel != null)
            {
                ChannelServices.UnregisterChannel(_channel);
                _channel = null;
            }
        }

        private static void _CreateRemoteService(string channelName)
        {
            _channel = new IpcServerChannel(
                new Dictionary<string, string>
                {
                    { "name", channelName },
                    { "portName", channelName },
                    { "exclusiveAddressUse", "false" },
                },
                new BinaryServerFormatterSinkProvider { TypeFilterLevel = TypeFilterLevel.Full });

            ChannelServices.RegisterChannel(_channel, true);
            RemotingServices.Marshal(new _IpcRemoteService(), _RemoteServiceName);
        }

        private static void _SignalFirstInstance(string channelName, IList<string> args)
        {
            var secondInstanceChannel = new IpcClientChannel();
            ChannelServices.RegisterChannel(secondInstanceChannel, true);

            string remotingServiceUrl = "ipc://" + channelName + "/" + _RemoteServiceName;

            // Obtain a reference to the remoting service exposed by the first instance of the application
            var firstInstanceRemoteServiceReference = (_IpcRemoteService)RemotingServices.Connect(typeof(_IpcRemoteService), remotingServiceUrl);

            // Pass along the current arguments to the first instance if it's up and accepting requests.
            if (firstInstanceRemoteServiceReference != null)
            {
                firstInstanceRemoteServiceReference.InvokeFirstInstance(args);
            }
        }

        private static void _ActivateFirstInstance(IList<string> args)
        {
            if (Application.Current != null && !Application.Current.Dispatcher.HasShutdownStarted)
            {
                var handler = SingleInstanceActivated;
                if (handler != null)
                {
                    handler(Application.Current, new SingleInstanceEventArgs { Args = args });
                }
            }
        }
    }
}
