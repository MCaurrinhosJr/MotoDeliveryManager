using NUnit.Framework;
using Moq;
using MotoDeliveryManager.Domain.Interfaces.Repositories;
using MotoDeliveryManager.Domain.Models;
using MotoDeliveryManager.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MotoDeliveryManager.Test
{
    [TestFixture]
    public class MotoTest
    {
        [Test]
        public async Task AddMoto_ValidMotoData_MotoAddedSuccessfullyAsync()
        {
            // Arrange
            var mockRepository = new Mock<IMotoRepository>();
            var motoService = new MotoService(mockRepository.Object);

            var motoToAdd = new Moto
            {
                Placa = "ABC1234",
                Marca = "Honda",
                Modelo = "CB300",
                Ano = "2020"
            };

            mockRepository.Setup(repo => repo.GetByPlacaAsync(motoToAdd.Placa))
                .ReturnsAsync(new List<Moto>());

            mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Moto>()))
                .Returns(Task.CompletedTask);

            // Act
            await motoService.AddMotoAsync(motoToAdd);

            // Assert
            mockRepository.Verify(repo => repo.AddAsync(motoToAdd), Times.Once);
        }

        [Test]
        public async Task GetMotos_FilterByPlate_ReturnFilteredMotos()
        {
            // Arrange
            var mockRepository = new Mock<IMotoRepository>();
            var motoService = new MotoService(mockRepository.Object);

            var plateToFilter = "ABC1234";
            var expectedMotos = new List<Moto>
            {
                new Moto { Placa = plateToFilter },
                new Moto { Placa = plateToFilter }
            };

            mockRepository.Setup(repo => repo.GetByPlacaAsync(plateToFilter))
                .ReturnsAsync(expectedMotos);

            // Act
            var result = await motoService.GetMotosByPlacaAsync(plateToFilter);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedMotos.Count, result.Count);
        }

        [Test]
        public async Task UpdateMoto_ChangePlateOnly_PlateUpdatedSuccessfully()
        {
            // Arrange
            var mockRepository = new Mock<IMotoRepository>();
            var motoService = new MotoService(mockRepository.Object);

            var existingMoto = new Moto
            {
                Id = 1,
                Placa = "ABC1234",
                Marca = "Honda",
                Modelo = "CB300",
                Ano = "2020"
            };

            var newPlate = "XYZ5678";

            mockRepository.Setup(repo => repo.GetByIdAsync(existingMoto.Id))
                .ReturnsAsync(existingMoto);

            mockRepository.Setup(repo => repo.GetByPlacaAsync(newPlate))
                .ReturnsAsync(new List<Moto>());

            // Act
            await motoService.UpdateMotoAsync(existingMoto.Id, new Moto { Placa = newPlate });

            // Assert
            var updatedMoto = await motoService.GetMotoByIdAsync(existingMoto.Id);
            Assert.AreEqual(newPlate, updatedMoto.Placa);
        }

        [Test]
        public async Task RemoveMoto_ValidMotoId_MotoRemovedSuccessfully()
        {
            // Arrange
            var mockRepository = new Mock<IMotoRepository>();
            var motoService = new MotoService(mockRepository.Object);

            var motoIdToRemove = 1;

            mockRepository.Setup(repo => repo.GetByIdAsync(motoIdToRemove))
                .ReturnsAsync(new Moto());

            mockRepository.Setup(repo => repo.RemoveAsync(motoIdToRemove))
                .Returns(Task.CompletedTask);

            // Act
            await motoService.RemoveMotoAsync(motoIdToRemove);

            // Assert
            mockRepository.Verify(repo => repo.RemoveAsync(motoIdToRemove), Times.Once);
        }
    }
}
