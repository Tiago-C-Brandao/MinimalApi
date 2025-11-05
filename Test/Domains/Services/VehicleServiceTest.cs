using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Services;
using MinimalApi.Infrastructure.Db;
using Test.Utils;

namespace Test.Domains.Services;

[TestClass]
[DoNotParallelize]
public class VehicleServiceTest
{
    [TestMethod]
    public void SaveVehicleTest()
    {
        // Arrange
        var context = ServiceHelper.CreateTestContext();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Vehicles");

        var vehicle = new Vehicle()
        {
            Id = 1,
            Name = "TestName",
            Brand = "TesteBrand",
            Year = 2022
        };

        var vehicleService = new VehicleService(context);

        // Act
        vehicleService.AddVehicle(vehicle);

        // Assert
        Assert.AreEqual(1, vehicleService.All(1).Count());
    }

    [TestMethod]
    public void GetVehicleByIdTest()
    {
        // Arrange
        var context = ServiceHelper.CreateTestContext();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Vehicles");
        var vehicle = new Vehicle()
        {
            Id = 1,
            Name = "TestName",
            Brand = "TestBrand",
            Year = 2022,
        };

        var vehicleService = new VehicleService(context);

        // Act
        vehicleService.AddVehicle(vehicle);
        var vehicleTest = vehicleService.GetVehicle(vehicle.Id);

        // Assert
        Assert.AreEqual(1, vehicleTest.Id);
    }
}
