using STI.Playground.ServiceBus.Scenario2.Domain.People;
using System;

namespace STI.Playground.ServiceBus.Scenario2.Infra.People
{
    public sealed class PeopleRepository : IPeopleRepository
    {
        private readonly IDocumentStoreHolder _store;

        public PeopleRepository(IDocumentStoreHolder store)
        {
            _store = store;
        }
        public Person Get(Guid id)
        {
            using (var session = _store.Open())
            {
                var person = session.Load<Person>(id);
                return person;
            }
                
        }

        public void Save(Person person)
        {
            using (var session = _store.Open())
            {
                session.Store(person);
                session.SaveChanges();
            }
        }
    }
}
