﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Configuration;
using MotoDeliveryManager.Domain.Interfaces.Repositories;
using MotoDeliveryManager.Domain.Models;
using MotoDeliveryManager.Domain.Services;
using MotoDeliveryManager.Domain.Services.FirebaseStorage;
using MotoDeliveryManager.Domain.Models.Enum;
using MotoDeliveryManager.Domain.Interfaces.Services;

namespace MotoDeliveryManager.Test
{
    [TestFixture]
    public class EntregadorServiceTests
    {
        [Test]
        public async Task AddEntregador_ValidEntregadorData_EntregadorAddedSuccessfully()
        {
            // Arrange
            var mockRepository = new Mock<IEntregadorRepository>();
            var mockFirebaseStorageService = new Mock<IFirebaseStorageService>();
            var mockLocacaoService = new Mock<ILocacaoService>();
            var mockPedidoService = new Mock<IPedidoService>();
            var entregadorService = new EntregadorService(mockRepository.Object, mockFirebaseStorageService.Object, mockLocacaoService.Object, mockPedidoService.Object);

            // Configure o mock do método UploadImageAsync para retornar uma URL simulada
            mockFirebaseStorageService.Setup(service => service.UploadImageAsync(It.IsAny<byte[]>()))
                .ReturnsAsync("url_da_imagem_cnh");

            var base64ImageData = "iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAAI0lEQVR42mNkAAIAAAoAAv/lxKUAAAAASUVORK5CYII=";
            var imageDataBytes = Convert.FromBase64String(base64ImageData);

            var entregador = new Entregador
            {
                Nome = "João",
                CNPJ = "79444529000154",
                NumeroCNH = "92670019154",
                DataNascimento = new DateTime(1990, 1, 1),
                TipoCNH = TipoCNH.A,
                CNHImage = new CNHImage { Data = imageDataBytes } // Dados simulados da imagem da CNH
            };

            mockRepository.Setup(repo => repo.GetByCnpjAsync(entregador.CNPJ))
                .ReturnsAsync((Entregador)null);

            mockRepository.Setup(repo => repo.GetByNumeroCnhAsync(entregador.NumeroCNH))
                .ReturnsAsync((Entregador)null);

            mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Entregador>()))
                .ReturnsAsync(entregador);

            // Act
            await entregadorService.AddEntregadorAsync(entregador);

            // Assert
            mockRepository.Verify(repo => repo.AddAsync(entregador), Times.Once);
        }

        [Test]
        public async Task UpdateEntregador_CnhImageUploaded_SuccessfullyUpdated()
        {
            // Arrange
            var mockRepository = new Mock<IEntregadorRepository>();
            var mockFirebaseStorageService = new Mock<IFirebaseStorageService>();
            var mockLocacaoService = new Mock<ILocacaoService>();
            var mockPedidoService = new Mock<IPedidoService>();
            var entregadorService = new EntregadorService(mockRepository.Object, mockFirebaseStorageService.Object, mockLocacaoService.Object, mockPedidoService.Object);

            // Configure o mock do método UploadImageAsync para retornar uma URL simulada
            mockFirebaseStorageService.Setup(service => service.UploadImageAsync(It.IsAny<byte[]>()))
                .ReturnsAsync("url_da_imagem_cnh");

            var base64ImageData = "iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAAI0lEQVR42mNkAAIAAAoAAv/lxKUAAAAASUVORK5CYII=";
            var imageDataBytes = Convert.FromBase64String(base64ImageData);

            var entregador = new Entregador
            {
                Id = 1,
                Nome = "Maria",
                CNPJ = "98765432109876",
                NumeroCNH = "987654321",
                DataNascimento = new DateTime(1995, 5, 5),
                TipoCNH = TipoCNH.B,
                CNHImage = new CNHImage { Data = imageDataBytes } // Dados simulados da imagem da CNH
            };

            mockRepository.Setup(repo => repo.GetByIdAsync(entregador.Id))
        .ReturnsAsync(entregador);

            mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Entregador>()));

            // Act
            await entregadorService.UpdateEntregadorAsync(entregador.Id, entregador);

            // Assert
            mockRepository.Verify(repo => repo.UpdateAsync(entregador), Times.Once);
        }

        [Test]
        public async Task RemoveEntregadorAsync_EntregadorExistsAndNoAssociations_EntregadorRemovedSuccessfully()
        {
            // Arrange
            var mockRepository = new Mock<IEntregadorRepository>();
            var mockLocacaoService = new Mock<ILocacaoService>();
            var mockPedidoService = new Mock<IPedidoService>();
            var entregadorService = new EntregadorService(mockRepository.Object, null, mockLocacaoService.Object, mockPedidoService.Object);

            int entregadorId = 1;

            // Mocking behavior for GetByIdAsync to return an existing entregador
            mockRepository.Setup(repo => repo.GetByIdAsync(entregadorId))
                .ReturnsAsync(new Entregador());

            // Mocking behavior for GetLocacoesByEntregadorIdAsync and GetPedidosByEntregadorIdAsync to return empty lists
            mockLocacaoService.Setup(service => service.GetLocacoesByEntregadorIdAsync(entregadorId))
                .ReturnsAsync(new List<Locacao>());
            mockPedidoService.Setup(service => service.GetPedidosByEntregadorIdAsync(entregadorId))
                .ReturnsAsync(new List<Pedido>());

            // Act
            await entregadorService.RemoveEntregadorAsync(entregadorId);

            // Assert
            mockRepository.Verify(repo => repo.RemoveAsync(entregadorId), Times.Once);
        }

        [Test]
        public void RemoveEntregadorAsync_EntregadorDoesNotExist_ThrowsKeyNotFoundException()
        {
            // Arrange
            var mockRepository = new Mock<IEntregadorRepository>();
            var mockLocacaoService = new Mock<ILocacaoService>();
            var mockPedidoService = new Mock<IPedidoService>();
            var entregadorService = new EntregadorService(mockRepository.Object, null, mockLocacaoService.Object, mockPedidoService.Object);

            int entregadorId = 1;

            // Mocking behavior for GetByIdAsync to return null, simulating that the entregador does not exist
            mockRepository.Setup(repo => repo.GetByIdAsync(entregadorId))
                .ReturnsAsync((Entregador)null);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await entregadorService.RemoveEntregadorAsync(entregadorId);
            });
        }

        [Test]
        public void RemoveEntregadorAsync_EntregadorHasAssociations_ThrowsInvalidOperationException()
        {
            // Arrange
            var mockRepository = new Mock<IEntregadorRepository>();
            var mockLocacaoService = new Mock<ILocacaoService>();
            var mockPedidoService = new Mock<IPedidoService>();
            var entregadorService = new EntregadorService(mockRepository.Object, null, mockLocacaoService.Object, mockPedidoService.Object);

            int entregadorId = 1;

            // Mocking behavior for GetByIdAsync to return an existing entregador
            mockRepository.Setup(repo => repo.GetByIdAsync(entregadorId))
                .ReturnsAsync(new Entregador());

            // Mocking behavior for GetLocacoesByEntregadorIdAsync and GetPedidosByEntregadorIdAsync to return non-empty lists, simulating associations
            mockLocacaoService.Setup(service => service.GetLocacoesByEntregadorIdAsync(entregadorId))
                .ReturnsAsync(new List<Locacao> { new Locacao() });
            mockPedidoService.Setup(service => service.GetPedidosByEntregadorIdAsync(entregadorId))
                .ReturnsAsync(new List<Pedido> { new Pedido() });

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await entregadorService.RemoveEntregadorAsync(entregadorId);
            });
        }
    }
}
