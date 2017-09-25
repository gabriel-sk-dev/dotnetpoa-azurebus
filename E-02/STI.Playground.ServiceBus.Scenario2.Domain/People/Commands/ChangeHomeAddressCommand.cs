using System;

namespace STI.Playground.ServiceBus.Scenario2.Domain.People.Commands
{
    public sealed class ChangeHomeAddressCommand
    {
        public ChangeHomeAddressCommand(Guid personId, Address newAddress)
        {
            PersonId = personId;
            NewAddress = newAddress;
        }
        public Guid PersonId { get; private set; }
        public Address NewAddress { get; private set; }

        public static ChangeHomeAddressCommand New(Guid personId, Address newAddress)
            => new ChangeHomeAddressCommand(personId, newAddress);
    }
}
