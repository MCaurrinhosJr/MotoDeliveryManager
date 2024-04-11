using NUnit.Framework;
using Moq;
using MotoDeliveryManager.Domain.Interfaces.Repositories;
using MotoDeliveryManager.Domain.Interfaces.Services;
using MotoDeliveryManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MotoDeliveryManager.Domain.Services;
using MotoDeliveryManager.Domain.Models.Enum;

namespace MotoDeliveryManager.Test
{
    [TestFixture]
    public class LocacaoServiceTests
    {
        [Test]
        public async Task AlugarMotoAsync_ValidAluguelRequest_ReturnsLocacao()
        {
            // Arrange
            var locacaoRepositoryMock = new Mock<ILocacaoRepository>();
            var locacaoService = new LocacaoService(locacaoRepositoryMock.Object);

            var aluguelRequest = new AluguelRequest
            {
                DataInicio = DateTime.Now,
                DataTerminoPrevista = DateTime.Now.AddDays(7),
                EntregadorId = 1,
                MotoId = 1
            };

            // Act
            var result = await locacaoService.AlugarMotoAsync(aluguelRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusLocacao.Ativa, result.Status);
        }

        [Test]
        public async Task DevolverMotoAsync_ValidDevolucaoRequest_ReturnsUpdatedLocacao()
        {
            // Arrange
            var locacaoRepositoryMock = new Mock<ILocacaoRepository>();
            var locacaoService = new LocacaoService(locacaoRepositoryMock.Object);

            var devolucaoRequest = new DevolucaoRequest
            {
                LocacaoId = 1,
                DataDevolucao = DateTime.Now.AddDays(7)
            };

            var locacao = new Locacao
            {
                Id = 1,
                DataInicio = DateTime.Now,
                DataTerminoPrevista = DateTime.Now.AddDays(7),
                ValorTotalPrecisto = 7 * 30,
                EntregadorId = 1,
                MotoId = 1,
                Status = StatusLocacao.Ativa
            };

            locacaoRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(locacao);

            // Act
            await locacaoService.DevolverMotoAsync(devolucaoRequest);

            // Assert
            locacaoRepositoryMock.Verify(repo => repo.UpdateAsync(locacao), Times.Once);
            Assert.AreEqual(StatusLocacao.Concluida, locacao.Status);
        }

        [Test]
        public async Task GetAllLocacoesAsync_ValidCall_ReturnsListOfLocacoes()
        {
            // Arrange
            var locacaoRepositoryMock = new Mock<ILocacaoRepository>();
            var locacaoService = new LocacaoService(locacaoRepositoryMock.Object);

            var locacoes = new List<Locacao>
            {
                new Locacao { Id = 1, Status = StatusLocacao.Ativa },
                new Locacao { Id = 2, Status = StatusLocacao.Concluida }
            };

            locacaoRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(locacoes);

            // Act
            var result = await locacaoService.GetAllLocacoesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(locacoes.Count, result.Count);
        }

        [Test]
        public async Task GetLocacaoByIdAsync_ExistingId_ReturnsLocacao()
        {
            // Arrange
            var locacaoRepositoryMock = new Mock<ILocacaoRepository>();
            var locacaoService = new LocacaoService(locacaoRepositoryMock.Object);

            var locacao = new Locacao { Id = 1, Status = StatusLocacao.Ativa };

            locacaoRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(locacao);

            // Act
            var result = await locacaoService.GetLocacaoByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(locacao.Id, result.Id);
        }
    }
}
