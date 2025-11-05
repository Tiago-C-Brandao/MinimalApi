using MinimalApi.Domain.Entities;

namespace Test.Domains.Entities;

[TestClass]
public class VehicleTest
{
    [TestMethod]
    public void TestVehicleGetSetProperty()
    {
        // Arrange
        
        var vehicle = new Vehicle();

        // Act

        vehicle.Id = 1;
        vehicle.Name = "TestName";
        vehicle.Brand = "TestBrand";
        vehicle.Year = 2022;

        // Assert

        Assert.AreEqual(1, vehicle.Id);
        Assert.AreEqual("TestName", vehicle.Name);
        Assert.AreEqual("TestBrand", vehicle.Brand);
        Assert.AreEqual(2022, vehicle.Year);
    }
}
