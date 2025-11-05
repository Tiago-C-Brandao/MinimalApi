using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Services;
using Test.Utils;


namespace Test.Domains.Services;

[TestClass]
[DoNotParallelize] 
public class AdminServiceTest
{

    [TestMethod]
    public void SaveAdminTest()
    {
        // Arrange
        var context = ServiceHelper.CreateTestContext();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Admins");
        
        var adm = new Admin();
        adm.Id = 1;
        adm.Email = "test@test.com";
        adm.Password = "test";
        adm.Role = "Adm";

        var admService = new AdminService(context);

        // Act
        admService.AddAdmin(adm);

        // Assert
        Assert.AreEqual(1, admService.All(1).Count());
    }

    [TestMethod]
    public void GetAdminByIdTest()
    {
        // Arrange
        var context = ServiceHelper.CreateTestContext();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Admins");

        var adm = new Admin();
        adm.Id = 1;
        adm.Email = "test@test.com";
        adm.Password = "test";
        adm.Role = "Adm";

        var admService = new AdminService(context);

        // Act
        admService.AddAdmin(adm);
        var admTest = admService.GetAdmin(adm.Id);

        // Assert
        Assert.AreEqual(1, admTest.Id);
    }
}
