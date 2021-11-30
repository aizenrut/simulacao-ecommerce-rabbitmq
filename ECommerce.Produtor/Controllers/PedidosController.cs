using ECommerce.Compartilhado;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace ECommerce.Produtor.Controllers
{
    public class PedidosController : ControllerBase
    {
        private IModel channel;
        private readonly ILogger<PedidosController> logger;

        public PedidosController(IModel channel, ILogger<PedidosController> logger)
        {
            this.channel = channel;
            this.logger = logger;
        }

        [HttpPost("SimularBlackFriday")]
        public IActionResult Cadastrar()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            for (int i = 0; i < 5000; i++)
            {
                logger.LogInformation($"{DateTime.Now:g} - Realizando pedido {i}");

                var pedido = new Pedido
                {
                    Id = i,
                    DataSolicitacao = DateTime.Now
                };

                var message = JsonConvert.SerializeObject(pedido);
                var bytes = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "pedidos",
                                     basicProperties: null,
                                     body: bytes);
            }

            return Ok();
        }

        [HttpGet("MediaTempo")]
        public IActionResult ObterMediaProcessamento()
        {
            return Ok(0);
        }
    }
}
