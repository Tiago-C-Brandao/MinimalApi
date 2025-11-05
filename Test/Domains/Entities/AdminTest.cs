using MinimalApi.Domain.Entities;

namespace Test.Domains.Entities;

[TestClass]
public class AdminTest
{
    [TestMethod]
    public void TestAdminGetSetProperty()
    {
        // Arrange
        var adm = new Admin();

        // Act
        adm.Id = 1;
        adm.Email = "test@test.com";
        adm.Password = "test";
        adm.Role = "Adm";

        // Assert
        Assert.AreEqual(1, adm.Id);
        Assert.AreEqual("test@test.com", adm.Email);
        Assert.AreEqual("test", adm.Password);
        Assert.AreEqual("Adm", adm.Role);
    }
}
