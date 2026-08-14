using FluentAssertions;

using GarageLog.Application.DTOs.Vehicle;
using GarageLog.Application.Interfaces;
using GarageLog.Application.Interfaces.Repositories;
using GarageLog.Application.Services;
using GarageLog.Core.Entities;
using GarageLog.Core.Enums;

using Moq;

namespace GarageLog.UnitTests.Services;

public class VehicleServiceTests
{
    private readonly Mock<IVehicleRepository> _vehicleRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly VehicleService _sut;

    public VehicleServiceTests()
    {
        _vehicleRepoMock = new Mock<IVehicleRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _sut = new VehicleService(_vehicleRepoMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CreateVehicleAsync_ValidRequest_ReturnsResponseAndSavesChanges()
    {
        // Arrange
        var request = new CreateVehicleRequest
        {
            Type = VehicleType.Car, Make = "Honda", Model = "Civic", Year = 2020,
        };

        // Act
        var result = await _sut.CreateVehicleAsync(request, userId: 1);

        // Assert
        result.Should().NotBeNull();
        result.Make.Should().Be("Honda");
        result.Model.Should().Be("Civic");
        result.Year.Should().Be(2020);
        result.Type.Should().Be(VehicleType.Car);
        _vehicleRepoMock.Verify(r => r.Add(It.IsAny<Vehicle>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetVehicleAsync_VehicleNotFound_ReturnsNull()
    {
        // Arrange
        _vehicleRepoMock.Setup(r => r.GetDetailsByIdAsync(1, 1)).ReturnsAsync((VehicleResponse?)null);

        // Act
        var result = await _sut.GetVehicleAsync(id: 1, userId: 1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetVehicleAsync_Found_ReturnsResponse()
    {
        // Arrange
        var response = new VehicleResponse
        {
            Id = 1,
            Type = VehicleType.Car,
            Make = "Honda",
            Model = "Civic",
            Year = 2020,
        };

        _vehicleRepoMock.Setup(r => r.GetDetailsByIdAsync(1, 1)).ReturnsAsync(response);

        // Act
        var result = await _sut.GetVehicleAsync(id: 1, userId: 1);

        // Assert
        result.Should().NotBeNull();
        result!.Make.Should().Be("Honda");
    }

    [Fact]
    public async Task GetVehiclesAsync_ReturnsAllVehiclesForUser()
    {
        // Arrange
        var vehicles = new List<VehicleResponse>
        {
            new()
            {
                Id = 1,
                Type = VehicleType.Car,
                Make = "Honda",
                Model = "Civic",
                Year = 2020
            },
            new()
            {
                Id = 2,
                Type = VehicleType.Car,
                Make = "Toyota",
                Model = "Camry",
                Year = 2019
            },
        };

        _vehicleRepoMock.Setup(r => r.GetAllByUserIdAsync(1)).ReturnsAsync(vehicles);

        // Act
        var result = await _sut.GetVehiclesAsync(userId: 1);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpdateVehicleAsync_VehicleNotFound_ReturnsNull()
    {
        // Arrange
        var request = CreateUpdateRequest();

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync((Vehicle?)null);

        // Act
        var result = await _sut.UpdateVehicleAsync(id: 1, request, userId: 1);

        // Assert
        result.Should().BeNull();
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateVehicleAsync_ValidRequest_UpdatesDetailsAndSaves()
    {
        // Arrange
        var vehicle = CreateVehicle();
        var request = CreateUpdateRequest(make: "Toyota", model: "Camry", year: 2021);

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(vehicle);

        // Act
        var result = await _sut.UpdateVehicleAsync(id: 1, request, userId: 1);

        // Assert
        result.Should().NotBeNull();
        result!.Make.Should().Be("Toyota");
        result.Model.Should().Be("Camry");
        result.Year.Should().Be(2021);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteVehicleAsync_VehicleNotFound_ReturnsFalse()
    {
        // Arrange
        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync((Vehicle?)null);

        // Act
        var result = await _sut.DeleteVehicleAsync(id: 1, userId: 1);

        // Assert
        result.Should().BeFalse();
        _vehicleRepoMock.Verify(r => r.Delete(It.IsAny<Vehicle>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteVehicleAsync_Found_DeletesAndReturnsTrue()
    {
        // Arrange
        var vehicle = CreateVehicle();

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(vehicle);

        // Act
        var result = await _sut.DeleteVehicleAsync(id: 1, userId: 1);

        // Assert
        result.Should().BeTrue();
        _vehicleRepoMock.Verify(r => r.Delete(vehicle), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    /* HELPER METHODS */
    private static Vehicle CreateVehicle(int userId = 1)
    {
        return Vehicle.Create(userId, VehicleType.Car, "Honda", "Civic", 2020);
    }

    private static UpdateVehicleRequest CreateUpdateRequest(
        string make = "Honda", string model = "Civic", int year = 2020)
    {
        return new UpdateVehicleRequest
        {
            Type = VehicleType.Car, Make = make, Model = model, Year = year,
        };
    }
}
