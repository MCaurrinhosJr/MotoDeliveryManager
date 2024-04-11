using MotoDeliveryManager.Domain.Interfaces.Services;
using MotoDeliveryManager.Domain.Models;
using MotoDeliveryManager.Domain.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MotoDeliveryManager.RabbitMqConsumer.Services
{
    public class RabbitMqConsumerService
    {
        private readonly IConfiguration _configuration;
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string FilaPedidos = "pedidos";
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMqConsumerService(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;

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

        public void StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received message: {message}");

                try
                {
                    await ProcessMessageAsync(message); // Chamar o método para processar a mensagem recebida
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString(), $"Error processing message: {message}");
                }
            };
            _channel.BasicConsume(queue: FilaPedidos,
                                  autoAck: true,
                                  consumer: consumer);
        }

        public bool TestarFilaAtiva()
        {
            var queueDeclareOk = _channel.QueueDeclarePassive("pedidos");

            return true;
        }

        public void StopConsuming()
        {
            // Lógica para parar o consumo, se necessário
        }

        private async Task ProcessMessageAsync(string message)
        {
            Pedido pedido = JsonConvert.DeserializeObject<Pedido>(message);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _notificacaoService = scope.ServiceProvider.GetRequiredService<INotificacaoService>();
                var _locacaoService = scope.ServiceProvider.GetRequiredService<ILocacaoService>();
                

                var entregadoresAptos = await ObterEntregadoresAptos(_locacaoService);

                foreach (var entregador in entregadoresAptos)
                {
                    string mensagemNotificacao = $"Novo pedido disponível: ID {pedido.Id}";

                    await _notificacaoService.EnviarNotificacaoAsync(entregador.Id, pedido.Id, mensagemNotificacao);
                }
            }
            
        }

        private async Task<List<Entregador>> ObterEntregadoresAptos(ILocacaoService _locacaoService)
        {
            var locacoesAtivas = await _locacaoService.GetAllLocacoesAsync();
            
            var entregadoresAtivos = locacoesAtivas.Where(l => l.Status == Domain.Models.Enum.StatusLocacao.Ativa).Select(l => l.Entregador).Distinct().ToList();

            return entregadoresAtivos;
        }
    }
}