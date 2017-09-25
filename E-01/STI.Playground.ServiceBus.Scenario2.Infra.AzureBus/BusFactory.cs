using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace STI.Playground.ServiceBus.Scenario2.Infra.AzureBus
{
    public class BusFactory
    {
        private readonly IUnityContainer _container;
        private string _azureStringConnection;
        private readonly IDictionary<string, Action<IServiceBusReceiveEndpointConfigurator>> _endpoints;

        private BusFactory(IUnityContainer container)
        {
            _container = container;
            _container.RegisterInstance<IUnityContainer>(_container);
            _endpoints = new Dictionary<string, Action<IServiceBusReceiveEndpointConfigurator>>();
        }

        public static IBus CreateSendOnly(string azureConnection)
            => new BusFactory(new UnityContainer())
                    .ConfigureCredential(azureConnection)
                    .Build();

        public static BusFactory New(IUnityContainer container, string azureConnection)
            => new BusFactory(container)
                    .ConfigureCredential(azureConnection);

        public BusFactory ConfigureCredential(string stringConnection)
        {
            _azureStringConnection = stringConnection;
            return this;
        }

        public BusFactory AddReceiveEndpoint(
            string queueName,
            Action<IServiceBusReceiveEndpointConfigurator> endpoint)
        {
            _endpoints.Add(queueName, endpoint);
            return this;
        }

        public IBus Build()
        {
            var busControl = Bus.Factory.CreateUsingAzureServiceBus(c =>
            {
                var host = c.Host(_azureStringConnection, x=> { });
                foreach (var endpoint in _endpoints)
                    c.ReceiveEndpoint(host, endpoint.Key, endpoint.Value);
            });
            var bus = new AzureBus(busControl);
            _container.RegisterInstance<IBus>(bus);
            return bus;
        }
    }
}
