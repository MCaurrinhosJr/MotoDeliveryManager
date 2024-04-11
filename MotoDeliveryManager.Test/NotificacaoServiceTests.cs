using Moq;
using MotoDeliveryManager.Domain.Interfaces.Repositories;
using MotoDeliveryManager.Domain.Interfaces.Services;
using MotoDeliveryManager.Domain.Models;
using MotoDeliveryManager.Domain.Models.Enum;
using MotoDeliveryManager.Domain.Services.RabbitMq;
using MotoDeliveryManager.Domain.Services;
using NUnit.Framework;
using System;
using System.Threading.Tasks;


namespace MotoDeliveryManager.Test
{
    [TestFixture]
    public class NotificacaoServiceTests
    {
        private Mock<INotificacaoRepository> _notificacaoRepositoryMock;
        private Mock<IEntregadorRepository> _entregadorRepositoryMock;
        private INotificacaoService _notificacaoService;

        [SetUp]
        public void Setup()
        {
            _notificacaoRepositoryMock = new Mock<INotificacaoRepository>();
            _entregadorRepositoryMock = new Mock<IEntregadorRepository>();
            _notificacaoService = new NotificacaoService(_notificacaoRepositoryMock.Object, _entregadorRepositoryMock.Object);
        }

        [Test]
        public async Task EnviarNotificacaoAsync_ValidInput_AddsNotificacao()
        {
            // Arrange
            _notificacaoRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Notificacao>())).Verifiable();
            var entregadorId = 1;
            var pedidoId = 1;
            var mensagem = "Teste de notificação";

            // Act
            await _notificacaoService.EnviarNotificacaoAsync(entregadorId, pedidoId, mensagem);

            // Assert
            _notificacaoRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Notificacao>()), Times.Once);
        }

        [Test]
        public async Task GetNotificacoesPorEntregadorAsync_ValidInput_ReturnsNotificacoes()
        {
            // Arrange
            var entregadorId = 1;
            var notificacoes = new List<Notificacao>
            {
                new Notificacao { Id = 1, Mensagem = "Teste 1", DataEnvio = DateTime.Now, EntregadorId = 1, PedidoId = 1 },
                new Notificacao { Id = 2, Mensagem = "Teste 2", DataEnvio = DateTime.Now, EntregadorId = 1, PedidoId = 2 }
            };
            _notificacaoRepositoryMock.Setup(repo => repo.GetNotificacoesPorEntregadorAsync(entregadorId)).ReturnsAsync(notificacoes);

            // Act
            var result = await _notificacaoService.GetNotificacoesPorEntregadorAsync(entregadorId);

            // Assert
            Assert.AreEqual(notificacoes.Count, result.Count);
        }

        [Test]
        public async Task GetEntregadoresNotificadosAsync_ValidInput_ReturnsEntregadores()
        {
            // Arrange
            var pedidoId = 1;
            var notificacoes = new List<Notificacao>
            {
                new Notificacao { Id = 1, Mensagem = "Teste 1", DataEnvio = DateTime.Now, EntregadorId = 1, PedidoId = pedidoId },
                new Notificacao { Id = 2, Mensagem = "Teste 2", DataEnvio = DateTime.Now, EntregadorId = 2, PedidoId = pedidoId }
            };
            var entregadores = new List<Entregador>
            {
                new Entregador { Id = 1, Nome = "Entregador 1" },
                new Entregador { Id = 2, Nome = "Entregador 2" }
            };
            _notificacaoRepositoryMock.Setup(repo => repo.GetNotificacoesPorPedidoAsync(pedidoId)).ReturnsAsync(notificacoes);
            _entregadorRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => entregadores.Find(e => e.Id == id));

            // Act
            var result = await _notificacaoService.GetEntregadoresNotificadosAsync(pedidoId);

            // Assert
            Assert.AreEqual(entregadores.Count, result.Count);
        }
    }
}
