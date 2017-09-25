using MassTransit;
using STI.Playground.ServiceBus.Scenario2.Domain.People.Events;
using STI.Playground.ServiceBus.Scenario2.Infra.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace STI.Playground.ServiceBus.Taxes.Application
{
    public sealed class CalculateNewTaxesFromChangedAddressService : IConsumer<HomeAddressChangedEvent>
    {
        private readonly IHubProxyFactory _notificationHub;

        public CalculateNewTaxesFromChangedAddressService(IHubProxyFactory notificationHub)
        {
            _notificationHub = notificationHub;
        }

        public async Task Consume(ConsumeContext<HomeAddressChangedEvent> context)
        {
            //Calculate new debts values
            var calculateProcessId = Guid.NewGuid();
            var notificationHub = _notificationHub
                                     .CreateProxy("http://localhost:2627", "playgroundServer");
            notificationHub.ProcessStarted(
                new { id = calculateProcessId, name = "debtsCalculationStarted", adtionalInfo = new { } });
            Thread.Sleep(5000);
            notificationHub.ProcessFinished(
                new { id = calculateProcessId, name = "finishedCalculationDebts", adtionalInfo = new { } });
        }
    }
}
