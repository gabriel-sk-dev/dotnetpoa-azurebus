using MassTransit;
using STI.Playground.ServiceBus.Scenario2.Domain.People.Events;
using System.Threading;
using System.Threading.Tasks;

namespace STI.Playground.ServiceBus.Taxes.Application
{
    public sealed class CalculateNewTaxesFromChangedAddressService : IConsumer<HomeAddressChangedEvent>
    {
        public async Task Consume(ConsumeContext<HomeAddressChangedEvent> context)
        {
            Thread.Sleep(5000);
        }
    }
}
