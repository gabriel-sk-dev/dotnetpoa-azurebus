using System;

namespace STI.Playground.ServiceBus.Scenario2.Domain.People
{
    public interface IPeopleRepository
    {
        void Save(Person person);
        Person Get(Guid id);
    }
}
