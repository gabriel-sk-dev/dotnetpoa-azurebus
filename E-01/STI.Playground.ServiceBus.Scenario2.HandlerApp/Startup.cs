using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Practices.Unity;
using STI.Playground.ServiceBus.Scenario2.Infra;
using STI.Playground.ServiceBus.Scenario2.Domain.People;
using STI.Playground.ServiceBus.Scenario2.Infra.People;
using STI.Playground.ServiceBus.Scenario2.Domain.People.Commands;
using STI.Playground.ServiceBus.Scenario2.Application.People;
using MassTransit;
using STI.Playground.ServiceBus.Scenario2.Infra.AzureBus;
using STI.Playground.ServiceBus.Taxes.Application;
using STI.Playground.ServiceBus.Notification.Application;
using STI.Playground.ServiceBus.Scenario2.Infra.Contracts;
using STI.Playground.ServiceBus.Scenario2.Infra.SignalR;
using MassTransit.AzureServiceBusTransport;

namespace STI.Playground.ServiceBus.Scenario2.HandlerApp
{
    public class Startup
    {
        public void ConfigureServices()
        {
            var azureStringConnection = "Endpoint={your namespace for azure service bus}/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey={Access key of RootManageSharedAccessKey}";
            var container = new UnityContainer();
            container.RegisterInstance<IDocumentStoreHolder>(
                new DocumentStoreHolder("{Raven db endpoint}"));
            container.RegisterType<IPeopleRepository, PeopleRepository>();
            container.RegisterType<IConsumer<ChangeHomeAddressCommand>, ChangeHomeAddressService>();
            container.RegisterType<IHubProxyFactory, HubProxyFactory>();

            var busControl = Bus.Factory.CreateUsingAzureServiceBus(c =>
            {
                var host = c.Host(azureStringConnection, x => { });
                
                c.ReceiveEndpoint(
                    host, 
                    "PeopleService-ChangeHomeAddress", 
                    endpoint => 
                    {
                        endpoint.Consumer(typeof(ChangeHomeAddressService), type => container.Resolve(type));
                    });
            });

            container.RegisterInstance<IBusControl>(busControl);
            container.RegisterInstance<MassTransit.IBus>(busControl);
            busControl.StartAsync();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
