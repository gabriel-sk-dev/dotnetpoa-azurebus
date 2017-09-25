using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using STI.Playground.ServiceBus.Scenario2.Infra.AzureBus;
using STI.Playground.ServiceBus.Scenario2.Infra.People;
using STI.Playground.ServiceBus.Scenario2.Domain.People;
using STI.Playground.ServiceBus.Scenario2.Infra;

namespace STI.Playground.ServiceBus.Scenario2.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDocumentStoreHolder>(new DocumentStoreHolder("{Raven db endpoint}"));
            services.AddTransient<IPeopleRepository, PeopleRepository>();
            var azureStringConnection = "Endpoint={your namespace for azure service bus}/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey={Access key of RootManageSharedAccessKey}";
            var bus = BusFactory
                        .CreateSendOnly(azureStringConnection);
            services.AddSingleton(bus);
            services.AddMvc();
            bus.StartAsync();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseCors(buider => buider
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
            );
            app.UseMvc();
        }
    }
}
