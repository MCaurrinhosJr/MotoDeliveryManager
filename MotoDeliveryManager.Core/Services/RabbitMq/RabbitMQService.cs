using Microsoft.Extensions.Configuration;
using MotoDeliveryManager.Domain.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace MotoDeliveryManager.Domain.Services.RabbitMq
{
    public class RabbitMQService
    {
        private readonly IConfiguration _configuration;
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string FilaPedidos = "pedidos";

        public RabbitMQService(IConfiguration configuration)
        {
            _configuration = configuration;

            // Obter as configurações do RabbitMQ do appsettings.json
            var rabbitMQConfig = _configuration.GetSection("ConnectionStrings:RabbitMQ");

            // Configurar a conexão com o RabbitMQ
            _connectionFactory = new ConnectionFactory
            {
                HostName = rabbitMQConfig["HostName"],
                UserName = rabbitMQConfig["UserName"],
                Password = rabbitMQConfig["Password"],
                Port = int.Parse(rabbitMQConfig["Port"])
            };

            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: FilaPedidos, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public bool TestarFilaAtiva()
        {
            // Verificar se a fila está declarada
            var queueDeclareOk = _channel.QueueDeclarePassive(FilaPedidos);

            // Se não houver exceção ao declarar a fila, então está ativa
            return true;
        }

        public void EnviarNotificacaoPedidoDisponivel(Pedido pedido)
        {
            var mensagem = JsonConvert.SerializeObject(pedido);
            EnviarMensagemParaFila(mensagem);
        }
        public void EnviarMensagemParaFila(string mensagem)
        {
            var body = Encoding.UTF8.GetBytes(mensagem);
            _channel.BasicPublish(exchange: "", routingKey: FilaPedidos, basicProperties: null, body: body);
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
