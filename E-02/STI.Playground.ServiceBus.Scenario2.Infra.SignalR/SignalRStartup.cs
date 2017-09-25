using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;

namespace STI.Playground.ServiceBus.Scenario2.Infra.SignalR
{
    public sealed class SignalRStartup
    {
        public static IDisposable RunSignalR(string address)
            => WebApp.Start<Startup>(new StartOptions(url: address));

        private sealed class Startup
        {
            public static void Configuration(IAppBuilder app)
            {
                app.UseWelcomePage("/");
                app.UseCors(CorsOptions.AllowAll);
                app.MapSignalR();
            }
        }
    }
}
