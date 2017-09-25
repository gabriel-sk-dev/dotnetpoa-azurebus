using MassTransit;
using STI.Playground.ServiceBus.Scenario2.Domain.People;
using STI.Playground.ServiceBus.Scenario2.Domain.People.Commands;
using STI.Playground.ServiceBus.Scenario2.Domain.People.Events;
using STI.Playground.ServiceBus.Scenario2.Infra.Contracts;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace STI.Playground.ServiceBus.Scenario2.Application.People
{
    public sealed class ChangeHomeAddressService : IConsumer<ChangeHomeAddressCommand>
    {
        private readonly IHubProxyFactory _notificationHub;
        private readonly MassTransit.IBus _bus;
        private readonly IPeopleRepository _peopleRepository;

        public ChangeHomeAddressService(
            IPeopleRepository peopleRepository,
            MassTransit.IBus bus,
            IHubProxyFactory notificationHub)
        {
            _peopleRepository = peopleRepository;
            _bus = bus;
            _notificationHub = notificationHub;
        }

        public async Task Consume(ConsumeContext<ChangeHomeAddressCommand> context)
        {
            try
            {
                Trace.TraceInformation($"Changing address for {context.Message.PersonId}");
                var person = _peopleRepository.Get(context.Message.PersonId);
                person.ChangeHomeAddress(context.Message.NewAddress);
                _peopleRepository.Save(person);

                var notificationHub = _notificationHub
                                        .CreateProxy("http://localhost:2627", "playgroundServer");
                //Calculate new debts values
                var calculateProcessId = Guid.NewGuid();
                notificationHub.ProcessStarted(
                    new { id = calculateProcessId, name = "debtsCalculationStarted", adtionalInfo = new { } });
                Thread.Sleep(5000);
                notificationHub.ProcessFinished(
                    new { id = calculateProcessId, name = "finishedCalculationDebts", adtionalInfo = new { } });

                //Send notification mail
                var emailProcessId = Guid.NewGuid();
                notificationHub.ProcessStarted(
                    new { id = emailProcessId, name = "sendingMail", adtionalInfo = new { } });
                Thread.Sleep(2000);
                notificationHub.ProcessFinished(
                    new { id = emailProcessId, name = "emailSent", adtionalInfo = new { } });

                notificationHub
                    .ProcessFinished(new
                    {
                        id = Guid.NewGuid(),
                        name = "addressSaved",
                        adtionalInfo = new { }
                    });
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }
    }
}
