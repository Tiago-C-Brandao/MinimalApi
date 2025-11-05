using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.ModelViews;
using System.Net;
using System.Text;
using System.Text.Json;
using Test.Helpers;

namespace Test;

[TestClass]
public class AdminRequestTest
{
    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        Setup.ClassInit(testContext);
    }

    [ClassCleanup]
    public static void ClassCleanup() 
    {
        Setup.ClassCleanup();
    }

    [TestMethod]
    public async Task TestGetSetProperties()
    {


        var loginDTO = new LoginDTO
        {
            Email = "admin@test.com",
            Password = "123456"
        };

        var adminLoggedIn = await LoginRequires.LoginTest(loginDTO);

        Assert.IsNotNull(adminLoggedIn?.Email ?? "");
        Assert.IsNotNull(adminLoggedIn?.Role ?? "");
        Assert.IsNotNull(adminLoggedIn?.Token ?? "");
    }

}
