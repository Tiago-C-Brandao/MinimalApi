using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.ModelViews;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Test.Helpers;

namespace Test;

[TestClass]
public class VehicleRequestTest
{
    [ClassInitialize]
    public static void ClassInicialize(TestContext context)
    {
        Setup.ClassInit(context);
    }

    [ClassCleanup]
    public static void ClassCleanup() 
    {
        Setup.ClassCleanup();
    }

    [TestMethod]
    public async Task TestPutVehicle()
    {
        // Arrange

        var loginDTO = new LoginDTO
        {
            Email = "editor@test.com",
            Password = "123456"
        };

        var adminLoggedIn = await LoginRequires.LoginTest(loginDTO);

        Assert.IsNotNull(adminLoggedIn?.Token ?? "");

        var vehicleDTO = new VehicleDTO
        {
            Name = "NewName",
            Brand = "NewBrand",
            Year = 2020,
        };

        // Act

        var contentVehicle = new StringContent(JsonSerializer.Serialize(vehicleDTO), Encoding.UTF8, "Application/json");

        // Add token in header
        Setup.client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", adminLoggedIn.Token);

        // Calls the endpoint that requires admin privilages.
        var responseVehicle = await Setup.client.PutAsync("vehicle/1", contentVehicle);

        // Assert
        Assert.AreEqual(HttpStatusCode.Forbidden, responseVehicle.StatusCode);
    }
}
