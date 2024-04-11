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
    public class PedidoServiceTests
    {
        [Test]
        public async Task AddPedido_ValidPedidoData_PedidoAddedSuccessfullyAsync()
        {
            // Arrange
            var _pedidoRepositoryMock = new Mock<IPedidoRepository>();
            var _rabbitMQServiceMock = new Mock<IRabbitMQService>();
            var _pedidoService = new PedidoService(_pedidoRepositoryMock.Object, _rabbitMQServiceMock.Object);

            var pedido = new Pedido
            {
                DataCriacao = DateTime.Now,
                ValorCorrida = 50,
                Endereco = "Rua A"
            };

            _pedidoRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Pedido>())).ReturnsAsync(pedido);
            _rabbitMQServiceMock.Setup(mock => mock.EnviarNotificacaoPedidoDisponivel(It.IsAny<Pedido>())).Returns(Task.CompletedTask);

            // Act
            await _pedidoService.AddPedidoAsync(pedido);

            // Assert
            _pedidoRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Pedido>()), Times.Once);
            _rabbitMQServiceMock.Verify(mock => mock.EnviarNotificacaoPedidoDisponivel(It.IsAny<Pedido>()), Times.Once);
        }

        [Test]
        public async Task AcceptPedido_ValidPedidoId_PedidoAcceptedSuccessfullyAsync()
        {
            // Arrange
            var _pedidoRepositoryMock = new Mock<IPedidoRepository>();
            var _rabbitMQServiceMock = new Mock<IRabbitMQService>();
            var _pedidoService = new PedidoService(_pedidoRepositoryMock.Object, _rabbitMQServiceMock.Object);

            var pedidoId = 1;
            var pedido = new Pedido
            {
                Id = pedidoId,
                DataCriacao = DateTime.Now,
                ValorCorrida = 50,
                Endereco = "Rua A"
            };

            _pedidoRepositoryMock.Setup(repo => repo.GetByIdAsync(pedidoId)).ReturnsAsync(pedido);
            _rabbitMQServiceMock.Setup(mock => mock.EnviarNotificacaoPedidoDisponivel(It.IsAny<Pedido>())).Returns(Task.CompletedTask);

            // Act
            await _pedidoService.AceitarPedidoAsync(pedidoId, pedido, 1);

            // Assert
            _pedidoRepositoryMock.Verify(repo => repo.UpdateAsync(pedido), Times.Once);
            _rabbitMQServiceMock.Verify(mock => mock.EnviarNotificacaoPedidoDisponivel(It.IsAny<Pedido>()), Times.Never);
        }

        [Test]
        public async Task CompletePedido_ValidPedidoId_PedidoCompletedSuccessfullyAsync()
        {
            // Arrange
            var _pedidoRepositoryMock = new Mock<IPedidoRepository>();
            var _rabbitMQServiceMock = new Mock<IRabbitMQService>();
            var _pedidoService = new PedidoService(_pedidoRepositoryMock.Object, _rabbitMQServiceMock.Object);

            var pedidoId = 1;
            var pedido = new Pedido
            {
                Id = pedidoId,
                DataCriacao = DateTime.Now,
                ValorCorrida = 50,
                Endereco = "Rua A",
                StatusPedido = StatusPedido.Aceito,
                EntregadorId = 1
            };

            _pedidoRepositoryMock.Setup(repo => repo.GetByIdAsync(pedidoId)).ReturnsAsync(pedido);
            _rabbitMQServiceMock.Setup(mock => mock.EnviarNotificacaoPedidoDisponivel(It.IsAny<Pedido>())).Returns(Task.CompletedTask);

            // Act
            await _pedidoService.EntregarPedidoAsync(pedidoId, pedido, 1);

            // Assert
            _pedidoRepositoryMock.Verify(repo => repo.UpdateAsync(pedido), Times.Once);
            _rabbitMQServiceMock.Verify(mock => mock.EnviarNotificacaoPedidoDisponivel(It.IsAny<Pedido>()), Times.Never);
        }
    }
}