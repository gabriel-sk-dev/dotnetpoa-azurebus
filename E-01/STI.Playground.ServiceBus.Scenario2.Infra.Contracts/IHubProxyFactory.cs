namespace STI.Playground.ServiceBus.Scenario2.Infra.Contracts
{
    public interface IHubProxyFactory
    {
        IHubProxy CreateProxy(string signalRAddress, string processName);
    }
}
