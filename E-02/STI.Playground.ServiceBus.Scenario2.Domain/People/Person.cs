using System;

namespace STI.Playground.ServiceBus.Scenario2.Domain.People
{
    public sealed class Person
    {
        public Person(Guid id, string publicId, FullName name, Address homeAddress )
        {
            Id = id;
            PublicId = publicId;
            Name = name;
            HomeAddress = homeAddress;
        }

        public Guid Id { get; private set; }
        public string PublicId { get; private set; }
        public FullName Name { get; private set; }
        public Address HomeAddress { get; private set; }

        public static Person New(string publicId, FullName name, Address homeAddress)
            => new Person(Guid.NewGuid(), publicId, name, homeAddress);

        public Person ChangeHomeAddress(Address newAddress)
        {
            if (newAddress.IsValid())
                throw new InvalidOperationException("Address is valid!");
            HomeAddress = newAddress;
            return this;
        }
    }
}
