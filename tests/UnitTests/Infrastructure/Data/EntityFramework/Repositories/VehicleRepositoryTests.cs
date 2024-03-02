using AutoFixture;
using Domain.Models.Vehicle;
using Domain.Models.Vehicle.ValueObjects;
using Domain.Repositories;
using FluentAssertions;
using Infrastructure.Data.EntityFramework.Repositories;

namespace UnitTests.Infrastructure.Data.EntityFramework.Repositories;

public class VehicleRepositoryTests : RepositoryTestsBase
{
    private readonly IVehicleRepository _repository;

    public VehicleRepositoryTests()
    {
        _repository = new VehicleRepository(AuctionDbContext);
    }
    
    [Fact]
        public async Task CreateVehicleAsync_ValidVehicle_ShouldReturnOkResult()
        {
            // Arrange
            var vehicle = Fixture.Build<Suv>().Without(x => x.Id).Create();

            // Act
            var result = await _repository.CreateVehicleAsync(vehicle);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().NotBe(default);
        }

        [Fact]
        public async Task GetVehicleByUniqueIdentifierAsync_ExistingVehicle_ShouldReturnVehicle()
        {
            // Arrange
            var existingVehicle = AuctionDbContext.Vehicles.First();

            // Act
            var result = await _repository.GetVehicleByUniqueIdentifierAsync(existingVehicle.UniqueIdentifier);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(existingVehicle);
        }

        [Fact]
        public async Task ListVehiclesAsync_FilteredQuery_ShouldReturnMatchingVehicles()
        {
            // Arrange

            // Act
            var result = await _repository.ListVehiclesAsync(null, null, null, null);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Any(x => x is Sedan).Should().BeTrue();
            result.Value.Any(x => x is Suv).Should().BeTrue();
            result.Value.Any(x => x is Truck).Should().BeTrue();
        }
}