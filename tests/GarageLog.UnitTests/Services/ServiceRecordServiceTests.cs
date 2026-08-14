using FluentAssertions;

using GarageLog.Application.DTOs.ServiceRecord;
using GarageLog.Application.Interfaces;
using GarageLog.Application.Interfaces.Repositories;
using GarageLog.Application.Services;
using GarageLog.Core.Entities;
using GarageLog.Core.Enums;

using Moq;

namespace GarageLog.UnitTests.Services;

public class ServiceRecordServiceTests
{
    private readonly Mock<IVehicleRepository> _vehicleRepoMock;
    private readonly Mock<IServiceRecordRepository> _serviceRecordRepoMock;
    private readonly Mock<IServiceTypeRepository> _serviceTypeRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ServiceRecordService _sut;

    public ServiceRecordServiceTests()
    {
        _vehicleRepoMock = new Mock<IVehicleRepository>();
        _serviceRecordRepoMock = new Mock<IServiceRecordRepository>();
        _serviceTypeRepoMock = new Mock<IServiceTypeRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _sut = new ServiceRecordService(
            _vehicleRepoMock.Object,
            _serviceRecordRepoMock.Object,
            _serviceTypeRepoMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_VehicleNotFound_ReturnsNull()
    {
        // Arrange
        var request = new CreateServiceRecordRequest
        {
            ServiceDate = new DateOnly(2026, 1, 15),
            Mileage = 50000,
            Items = [new CreateServiceRecordItemRequest { ServiceTypeId = 1 }],
        };

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((Vehicle?)null);

        // Act
        var result = await _sut.CreateAsync(vehicleId: 1, userId: 1, request);

        // Assert
        result.Should().BeNull();
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_NoServiceItems_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new CreateServiceRecordRequest
        {
            ServiceDate = new DateOnly(2026, 1, 15), Mileage = 50000, Items = [], // empty
        };

        // Act
        var act = async () => await _sut.CreateAsync(vehicleId: 1, userId: 1, request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*at least one*");
    }

    [Fact]
    public async Task CreateAsync_NoCustomName_ThrowsArgumentException()
    {
        // Arrange
        var vehicle = CreateVehicle();
        var customServiceType = CreateServiceTypeWithId(ServiceRecordItem.CustomServiceTypeId);

        var request = new CreateServiceRecordRequest
        {
            ServiceDate = new DateOnly(2026, 1, 15),
            IsSelfService = true,
            Mileage = 50000,
            Items =
            [
                new CreateServiceRecordItemRequest
                {
                    ServiceTypeId = ServiceRecordItem.CustomServiceTypeId, CustomName = null
                }
            ],
        };

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(vehicle);
        _serviceRecordRepoMock
            .Setup(r => r.GetPreviousRecordAsync(vehicle.Id, request.ServiceDate, null))
            .ReturnsAsync((ServiceRecord?)null);
        _serviceRecordRepoMock
            .Setup(r => r.GetNextRecordAsync(vehicle.Id, request.ServiceDate, null))
            .ReturnsAsync((ServiceRecord?)null);
        _serviceTypeRepoMock
            .Setup(r => r.GetByIdAsync(ServiceRecordItem.CustomServiceTypeId))
            .ReturnsAsync(customServiceType);

        // Act
        var act = async () => await _sut.CreateAsync(vehicleId: 1, userId: 1, request);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("*custom service name*");
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_ReturnsResponseAndSavesChanges()
    {
        // Arrange
        var vehicle = CreateVehicle();
        var serviceType = new ServiceType("Oil Change", "Maintenance");

        var request = new CreateServiceRecordRequest
        {
            ServiceDate = new DateOnly(2026, 1, 15),
            Mileage = 50000,
            IsSelfService = true,
            TotalCost = 45.99m,
            Items = [new CreateServiceRecordItemRequest { ServiceTypeId = 1 }],
        };

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(vehicle);
        _serviceRecordRepoMock
            .Setup(r => r.GetPreviousRecordAsync(vehicle.Id, request.ServiceDate, null))
            .ReturnsAsync((ServiceRecord?)null);
        _serviceRecordRepoMock
            .Setup(r => r.GetNextRecordAsync(vehicle.Id, request.ServiceDate, null))
            .ReturnsAsync((ServiceRecord?)null);
        _serviceTypeRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(serviceType);

        // Act
        var result = await _sut.CreateAsync(vehicleId: 1, userId: 1, request);

        // Assert
        result.Should().NotBeNull();
        result!.Mileage.Should().Be(50000);
        result.TotalCost.Should().Be(45.99m);
        result.IsSelfService.Should().BeTrue();
        result.Items.Should().ContainSingle(i => i.ServiceTypeName == "Oil Change");
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ServiceTypeNotFound_ThrowsInvalidOperationException()
    {
        // Arrange
        var vehicle = CreateVehicle();

        var request = new CreateServiceRecordRequest
        {
            ServiceDate = new DateOnly(2026, 1, 15),
            Mileage = 50000,
            IsSelfService = true,
            Items = [new CreateServiceRecordItemRequest { ServiceTypeId = 999 }],
        };

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(vehicle);
        _serviceRecordRepoMock
            .Setup(r => r.GetPreviousRecordAsync(vehicle.Id, request.ServiceDate, null))
            .ReturnsAsync((ServiceRecord?)null);
        _serviceRecordRepoMock
            .Setup(r => r.GetNextRecordAsync(vehicle.Id, request.ServiceDate, null))
            .ReturnsAsync((ServiceRecord?)null);
        _serviceTypeRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((ServiceType?)null);

        // Act
        var act = async () => await _sut.CreateAsync(vehicleId: 1, userId: 1, request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*999*");
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_MileageLowerThanPreviousRecord_ThrowsInvalidOperationException()
    {
        // Arrange
        var vehicle = CreateVehicle();
        var previous = vehicle.AddServiceRecord(new DateOnly(2026, 1, 1), mileage: 60000, isSelfService: true);

        var request = new CreateServiceRecordRequest
        {
            ServiceDate = new DateOnly(2026, 1, 15),
            Mileage = 50000, // lower than previous
            IsSelfService = true,
            Items = [new CreateServiceRecordItemRequest { ServiceTypeId = 1 }],
        };

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(vehicle);
        _serviceRecordRepoMock
            .Setup(r => r.GetPreviousRecordAsync(vehicle.Id, request.ServiceDate, null))
            .ReturnsAsync(previous);
        _serviceRecordRepoMock
            .Setup(r => r.GetNextRecordAsync(vehicle.Id, request.ServiceDate, null))
            .ReturnsAsync((ServiceRecord?)null);

        // Act
        var act = async () => await _sut.CreateAsync(vehicleId: 1, userId: 1, request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*previous*");
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_MileageGreaterThanNextRecord_ThrowsInvalidOperationException()
    {
        // Arrange
        var vehicle = CreateVehicle();
        var next = vehicle.AddServiceRecord(new DateOnly(2026, 1, 20), mileage: 40000, isSelfService: true);

        var request = new CreateServiceRecordRequest
        {
            ServiceDate = new DateOnly(2026, 1, 15),
            Mileage = 50000, // higher than next
            IsSelfService = true,
            Items = [new CreateServiceRecordItemRequest { ServiceTypeId = 1 }],
        };

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(vehicle);
        _serviceRecordRepoMock
            .Setup(r => r.GetPreviousRecordAsync(vehicle.Id, request.ServiceDate, null))
            .ReturnsAsync((ServiceRecord?)null);
        _serviceRecordRepoMock
            .Setup(r => r.GetNextRecordAsync(vehicle.Id, request.ServiceDate, null))
            .ReturnsAsync(next);

        // Act
        var act = async () => await _sut.CreateAsync(vehicleId: 1, userId: 1, request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*next*");
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_VehicleNotFound_ReturnsNull()
    {
        // Arrange
        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync((Vehicle?)null);

        // Act
        var result = await _sut.GetByIdAsync(vehicleId: 1, serviceRecordId: 1, userId: 1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ServiceRecordNotFound_ReturnsNull()
    {
        // Arrange
        var vehicle = CreateVehicle();

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(vehicle);
        _serviceRecordRepoMock
            .Setup(r => r.GetByIdAsync(vehicle.Id, 1))
            .ReturnsAsync((ServiceRecord?)null);

        // Act
        var result = await _sut.GetByIdAsync(vehicleId: 1, serviceRecordId: 1, userId: 1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_Found_ReturnsResponse()
    {
        // Arrange
        var vehicle = CreateVehicle();
        var serviceRecord = vehicle.AddServiceRecord(new DateOnly(2026, 1, 15), mileage: 50000, isSelfService: true);

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(vehicle);
        _serviceRecordRepoMock
            .Setup(r => r.GetByIdAsync(vehicle.Id, 1))
            .ReturnsAsync(serviceRecord);

        // Act
        var result = await _sut.GetByIdAsync(vehicleId: 1, serviceRecordId: 1, userId: 1);

        // Assert
        result.Should().NotBeNull();
        result!.Mileage.Should().Be(50000);
    }

    [Fact]
    public async Task GetAllAsync_VehicleNotFound_ReturnsNull()
    {
        // Arrange
        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync((Vehicle?)null);

        // Act
        var result = await _sut.GetAllAsync(vehicleId: 1, userId: 1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_VehicleFound_ReturnsAllRecords()
    {
        // Arrange
        var vehicle = CreateVehicle();
        var record1 = vehicle.AddServiceRecord(new DateOnly(2026, 1, 1), mileage: 40000, isSelfService: true);
        var record2 = vehicle.AddServiceRecord(new DateOnly(2026, 2, 1), mileage: 41000, isSelfService: true);

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(vehicle);
        _serviceRecordRepoMock
            .Setup(r => r.GetAllByVehicleIdAsync(1))
            .ReturnsAsync([record1, record2]);

        // Act
        var result = await _sut.GetAllAsync(vehicleId: 1, userId: 1);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllAsync_NoRecords_ReturnsEmptyCollection()
    {
        // Arrange
        var vehicle = CreateVehicle();

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(vehicle);
        _serviceRecordRepoMock
            .Setup(r => r.GetAllByVehicleIdAsync(1))
            .ReturnsAsync([]);

        // Act
        var result = await _sut.GetAllAsync(vehicleId: 1, userId: 1);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateAsync_VehicleNotFound_ReturnsNull()
    {
        // Arrange
        var request = CreateUpdateRequest();

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync((Vehicle?)null);

        // Act
        var result = await _sut.UpdateAsync(vehicleId: 1, serviceRecordId: 1, userId: 1, request);

        // Assert
        result.Should().BeNull();
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ServiceRecordNotFound_ReturnsNull()
    {
        // Arrange
        var vehicle = CreateVehicle();
        var request = CreateUpdateRequest();

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(vehicle);
        _serviceRecordRepoMock
            .Setup(r => r.GetByIdAsync(1, 1))
            .ReturnsAsync((ServiceRecord?)null);

        // Act
        var result = await _sut.UpdateAsync(vehicleId: 1, serviceRecordId: 1, userId: 1, request);

        // Assert
        result.Should().BeNull();
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ServiceTypeNotFound_ThrowsInvalidOperationException()
    {
        // Arrange
        var vehicle = CreateVehicle();
        var serviceRecord = vehicle.AddServiceRecord(new DateOnly(2026, 1, 15), mileage: 50000, isSelfService: true);
        var request = CreateUpdateRequest(serviceTypeId: 999);

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(vehicle);
        _serviceRecordRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(serviceRecord);
        _serviceRecordRepoMock
            .Setup(r => r.GetPreviousRecordAsync(vehicle.Id, request.ServiceDate, serviceRecord.Id))
            .ReturnsAsync((ServiceRecord?)null);
        _serviceRecordRepoMock
            .Setup(r => r.GetNextRecordAsync(vehicle.Id, request.ServiceDate, serviceRecord.Id))
            .ReturnsAsync((ServiceRecord?)null);
        _serviceTypeRepoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((ServiceType?)null);

        // Act
        var act = async () => await _sut.UpdateAsync(vehicleId: 1, serviceRecordId: 1, userId: 1, request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*999*");
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ValidRequest_UpdatesDetailsReplacesItemsAndSaves()
    {
        // Arrange
        var vehicle = CreateVehicle();
        var serviceRecord = vehicle.AddServiceRecord(new DateOnly(2026, 1, 15), mileage: 50000, isSelfService: true);
        var serviceType = new ServiceType("Tire Rotation", "Maintenance");
        var request = CreateUpdateRequest(mileage: 51000);

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(vehicle);
        _serviceRecordRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(serviceRecord);
        _serviceRecordRepoMock
            .Setup(r => r.GetPreviousRecordAsync(vehicle.Id, request.ServiceDate, serviceRecord.Id))
            .ReturnsAsync((ServiceRecord?)null);
        _serviceRecordRepoMock
            .Setup(r => r.GetNextRecordAsync(vehicle.Id, request.ServiceDate, serviceRecord.Id))
            .ReturnsAsync((ServiceRecord?)null);
        _serviceTypeRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(serviceType);

        // Act
        var result = await _sut.UpdateAsync(vehicleId: 1, serviceRecordId: 1, userId: 1, request);

        // Assert
        result.Should().NotBeNull();
        result!.Mileage.Should().Be(51000);
        result.Items.Should().ContainSingle(i => i.ServiceTypeName == "Tire Rotation");
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_VehicleNotFound_ReturnsFalse()
    {
        // Arrange
        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync((Vehicle?)null);

        // Act
        var result = await _sut.DeleteAsync(vehicleId: 1, serviceRecordId: 1, userId: 1);

        // Assert
        result.Should().BeFalse();
        _serviceRecordRepoMock.Verify(r => r.Delete(It.IsAny<ServiceRecord>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ServiceRecordNotFound_ReturnsFalse()
    {
        // Arrange
        var vehicle = CreateVehicle();

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(vehicle);
        _serviceRecordRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync((ServiceRecord?)null);

        // Act
        var result = await _sut.DeleteAsync(vehicleId: 1, serviceRecordId: 1, userId: 1);

        // Assert
        result.Should().BeFalse();
        _serviceRecordRepoMock.Verify(r => r.Delete(It.IsAny<ServiceRecord>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_Found_DeletesAndReturnsTrue()
    {
        // Arrange
        var vehicle = CreateVehicle();
        var serviceRecord = vehicle.AddServiceRecord(new DateOnly(2026, 1, 15), mileage: 50000, isSelfService: true);

        _vehicleRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(vehicle);
        _serviceRecordRepoMock.Setup(r => r.GetByIdAsync(1, 1)).ReturnsAsync(serviceRecord);

        // Act
        var result = await _sut.DeleteAsync(vehicleId: 1, serviceRecordId: 1, userId: 1);

        // Assert
        result.Should().BeTrue();
        _serviceRecordRepoMock.Verify(r => r.Delete(serviceRecord), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    /* HELPER METHODS */
    private static Vehicle CreateVehicle(int userId = 1)
    {
        return Vehicle.Create(userId, VehicleType.Car, "Honda", "Civic", 2020);
    }

    private static ServiceType CreateServiceTypeWithId(int id, string name = "Custom", string category = "Custom")
    {
        var serviceType = new ServiceType(name, category);
        typeof(ServiceType).GetProperty(nameof(ServiceType.Id))!.SetValue(serviceType, id);
        return serviceType;
    }

    private static UpdateServiceRecordRequest CreateUpdateRequest(int mileage = 50000, int serviceTypeId = 1)
    {
        return new UpdateServiceRecordRequest
        {
            ServiceDate = new DateOnly(2026, 1, 15),
            Mileage = mileage,
            IsSelfService = true,
            Items = [new CreateServiceRecordItemRequest { ServiceTypeId = serviceTypeId }],
        };
    }
}
