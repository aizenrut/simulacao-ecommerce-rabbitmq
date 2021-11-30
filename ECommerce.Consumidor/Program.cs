using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace ECommerce.Consumidor
{
    public class Program
    {
        private static IConnection currentConnection;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(factory =>
                    {
                        return new ConnectionFactory
                        {
                            HostName = "localhost",
                            UserName = "guest",
                            Password = "guest"
                        };
                    });

                    services.AddTransient(factory =>
                    {
                        if (currentConnection == null || !currentConnection.IsOpen)
                        {
                            var connectionfactory = factory.GetService<ConnectionFactory>();
                            currentConnection = connectionfactory.CreateConnection();
                        }

                        return currentConnection;
                    });

                    services.AddTransient(factory =>
                    {
                        var connection = factory.GetService<IConnection>();
                        return connection.CreateModel();
                    });

                    services.AddHostedService<Worker>();
                });
    }
}
