using Microsoft.AspNet.SignalR;
using System.Diagnostics;

namespace STI.Playground.ServiceBus.Scenario2.Infra.SignalR
{
    public class NotificationHub : Hub
    {
        public void NotifyProgress(object message)
        {
            Clients.All.notifyProgress(message);
        }

        public void ProcessStarted(object message)
        {
            Clients.All.processStarted(message);
        }

        public void ProcessFinished(object message)
        {
            Clients.All.processFinished(message);
        }

        public void ProcessSent(object message)
        {
            Trace.TraceInformation($"SignalR: ProcessSent with {message}");
            Clients.All.processSent(message);
        }
    }
}
