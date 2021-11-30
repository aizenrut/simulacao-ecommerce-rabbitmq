using ECommerce.Compartilhado;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Consumidor
{
    public class Worker : BackgroundService
    {
        private readonly IModel channel;
        private readonly ILogger<Worker> logger;

        public Worker(ILogger<Worker> logger, IModel channel)
        {
            this.channel = channel;
            this.logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += Processar;

            channel.BasicQos(0, 1, false);
            channel.BasicConsume(queue: "pedidos",
                                 autoAck: false,
                                 consumer: consumer);

            return Task.CompletedTask;
        }

        public void Processar(object model, BasicDeliverEventArgs eventArgs)
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var pedido = JsonConvert.DeserializeObject<Pedido>(message);

            Task.Delay(200).GetAwaiter().GetResult();

            var tempo = DateTime.Now - pedido.DataSolicitacao;

            logger.LogInformation($"{DateTime.Now:g} - Processando pedido {pedido.Id}, levou {tempo.TotalSeconds} segundos desde a realização");

            channel.BasicAck(eventArgs.DeliveryTag, false);
        }
    }
}
