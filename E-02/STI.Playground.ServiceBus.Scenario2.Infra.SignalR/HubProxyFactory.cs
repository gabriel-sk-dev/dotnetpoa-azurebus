using STI.Playground.ServiceBus.Scenario2.Infra.Contracts;
using System.Collections.Concurrent;
using System.Linq;

namespace STI.Playground.ServiceBus.Scenario2.Infra.SignalR
{
    public sealed class HubProxyFactory : IHubProxyFactory
    {
        private static readonly ConcurrentDictionary<string, IHubProxy> _proxys = new ConcurrentDictionary<string, IHubProxy>();

        public IHubProxy CreateProxy(string signalRAddress, string processName)
        {
            if (!_proxys.Any(x => x.Key.Equals(processName)))
                _proxys.TryAdd(processName, new HubProxy(signalRAddress, nameof(NotificationHub)));
            return _proxys[processName];
        }
    }
}
