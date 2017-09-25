using MassTransit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace STI.Playground.ServiceBus.Scenario2.Infra.AzureBus
{
    public interface IBus : IDisposable
    {
        void SendCommand<TCommand>(TCommand commandMessage) where TCommand : class;
        void RaiseEvent<T>(T eventMessage) where T : class;

        Task StartAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task StopAsync(CancellationToken cancellationToken = default(CancellationToken));
    }

    public class AzureBus : IBus
    {
        private readonly IBusControl _busControl;
        private BusHandle _busHandle;
        public AzureBus(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public void Dispose()
        { }

        public void RaiseEvent<T>(T eventMessage) where T : class
         => _busControl.Publish(eventMessage);

        public void SendCommand<TCommand>(TCommand commandMessage) where TCommand : class
            => _busControl.Publish(commandMessage);

        public async Task StartAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _busHandle = await _busControl.StartAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken = default(CancellationToken))
            => await _busHandle.StopAsync(cancellationToken).ConfigureAwait(false);
    }
}
