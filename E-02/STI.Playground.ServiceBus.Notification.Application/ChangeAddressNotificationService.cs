using MassTransit;
using STI.Playground.ServiceBus.Scenario2.Domain.People.Events;
using STI.Playground.ServiceBus.Scenario2.Infra.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace STI.Playground.ServiceBus.Notification.Application
{
    public sealed class ChangeAddressNotificationService : IConsumer<HomeAddressChangedEvent>
    {
        private readonly IHubProxyFactory _notificationHub;

        public ChangeAddressNotificationService(IHubProxyFactory notificationHub)
        {
           _notificationHub = notificationHub;
        }

        public async Task Consume(ConsumeContext<HomeAddressChangedEvent> context)
        {
            //Send notification mail
            var emailProcessId = Guid.NewGuid();
            var notificationHub = _notificationHub
                                     .CreateProxy("http://localhost:2627", "playgroundServer");
            notificationHub.ProcessStarted(
                new { id = emailProcessId, name = "sendingMail", adtionalInfo = new { } });
            Thread.Sleep(2000);
            notificationHub.ProcessFinished(
                new { id = emailProcessId, name = "emailSent", adtionalInfo = new { } });
        }
    }
}
