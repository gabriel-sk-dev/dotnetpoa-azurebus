namespace STI.Playground.ServiceBus.Scenario2.Infra.Contracts
{
    public interface IHubProxy
    {
        void ProcessStarted(object message);
        void NotifyProgress(object message);
        void ProcessFinished(object message);
        void ProcessSent(object message);
    }
}
