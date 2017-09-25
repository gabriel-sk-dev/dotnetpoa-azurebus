using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STI.Playground.ServiceBus.Scenario2.SignalRClient
{
    using static Console;
    class Program
    {
        static void Main(string[] args)
        {
            Write("Informe o endereço do signalR: ");
            var address = ReadLine();

            try
            {
                using (var hubConnection = new HubConnection(address))
                {
                    var hubProxy = hubConnection.CreateHubProxy("NotificationHub");
                    hubConnection.StateChanged += HubConnection_StateChanged;
                    hubConnection.Error += HubConnection_Error;
                    hubConnection.Start().Wait();

                    var canRun = true;
                    while (canRun)
                    {
                        Write("Digite a mensagem para enviar:");
                        var message = ReadLine();

                        hubProxy.Invoke("NotifyProgress", new { id = Guid.NewGuid().ToString(), name = "Teste", percentage = 15 }).Wait();

                        WriteLine("Mensagem enviada...");
                        WriteLine();

                        Write("Sair (S/N)? ");
                        var readed = ReadKey();

                        if (readed.Key == ConsoleKey.S)
                            canRun = false;
                    }

                    WriteLine("Programa finalizado ... pressione qualque tecla para sair");
                    ReadKey();
                }

            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
                ReadLine();
            }

        }

        private static void HubConnection_Error(Exception obj)
        {
            WriteLine($"Erro no HUB => {obj.Message}");
        }

        private static void HubConnection_StateChanged(StateChange obj)
        {
            WriteLine($"Conexão com HUB afetada => {obj.OldState.ToString()} para {obj.NewState.ToString()}");
        }
    }
}
