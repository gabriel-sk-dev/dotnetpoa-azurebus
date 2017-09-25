using STI.Playground.ServiceBus.Scenario2.Infra.Contracts;
using System.Threading;

namespace STI.Playground.ServiceBus.Scenario2.Infra.SignalR
{
    public sealed class HubProxy : IHubProxy
    {
        private readonly Microsoft.AspNet.SignalR.Client.HubConnection _connection;
        private readonly Microsoft.AspNet.SignalR.Client.IHubProxy _proxy;

        public HubProxy(string address, string hubName)
        {
            _connection = new Microsoft.AspNet.SignalR.Client.HubConnection(address);
            _proxy = _connection.CreateHubProxy(hubName);
            _connection.Start().Wait();
        }

        public void NotifyProgress(object message) => Notify(nameof(NotifyProgress), message);

        public void ProcessStarted(object message) => Notify(nameof(ProcessStarted), message);

        public void ProcessFinished(object message) => Notify(nameof(ProcessFinished), message);

        public void ProcessSent(object message) => Notify(nameof(ProcessSent), message);

        private void Notify(string method, object message, int attempt = 0)
        {
            attempt++;
            if (attempt > 5)
                return;//GABRIEL: logar falha no envio
            if (_connection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connecting
                    || _connection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Reconnecting)
            {
                Thread.Sleep(1000);
                Notify(method, method, attempt);
            }
            if (_connection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected)
            {
                _connection.Start().Wait();
                Notify(method, method, attempt);
            }

            if (_connection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                _proxy.Invoke(method, message).Wait();
        }
    }
}
