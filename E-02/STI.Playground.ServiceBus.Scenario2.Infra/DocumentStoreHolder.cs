using Raven.Client;
using Raven.Client.Converters;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Imports.Newtonsoft.Json;
using STI.Playground.ServiceBus.Scenario2.Domain.People;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace STI.Playground.ServiceBus.Scenario2.Infra
{
    public interface IDocumentStoreHolder
    {
        IDocumentSession Open();
    }

    public sealed class DocumentStoreHolder: IDocumentStoreHolder
    {
        private readonly DocumentStore _store;

        public DocumentStoreHolder(string urlDatabase)
        {
            _store = new DocumentStore { Url = urlDatabase };
            _store.Initialize();
            _store
                 .DatabaseCommands
                 .GlobalAdmin
                 .EnsureDatabaseExists($"AzureServiceBusDemo", ignoreFailures: false);
            _store.DefaultDatabase = $"AzureServiceBusDemo";

            var asm = Assembly.GetExecutingAssembly();
            IndexCreation.CreateIndexes(asm, _store);

            _store.Conventions.IdentityTypeConvertors = new List<ITypeConverter>
            {
                new GuidConverter()
            };

            _store.Conventions.FindTypeTagName = t =>
            {
                return t.Name == nameof(Person) ? nameof(People) : Raven.Client.Util.Inflector.Pluralize(t.Name);
            };

            var _serializer = _store.Conventions.CreateSerializer();
            _serializer.TypeNameHandling = TypeNameHandling.All;
        }

        public IDocumentSession Open()
            => _store.OpenSession();
    }
}
