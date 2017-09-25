using System;

namespace STI.Playground.ServiceBus.Scenario2.Domain.People.Events
{
    public sealed class HomeAddressChangedEvent
    {
        public HomeAddressChangedEvent(Guid personId)
        {
            PersonId = personId;
        }
        public Guid PersonId { get; set; }

        public static HomeAddressChangedEvent New(Guid personId)
            => new HomeAddressChangedEvent(personId);
    }
}
