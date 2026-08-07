using FluentAssertions;
using GarageLog.Application.DTOs.ServiceRecord;
using GarageLog.Application.Interfaces;
using GarageLog.Application.Interfaces.Repositories;
using GarageLog.Application.Services;
using GarageLog.Core.Entities;
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
            ServiceDate = new DateOnly(2026, 1, 15),
            Mileage = 50000,
            Items = [], // empty
        };

        // Act
        var act = async () => await _sut.CreateAsync(vehicleId: 1, userId: 1, request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*at least one*");
    }
}
