using Microsoft.AspNetCore.Identity.Data;
using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Test.Helpers
{
    internal class LoginRequires
    {
        public async static Task<AdminLoggedIn> LoginTest(LoginDTO loginDTO)
        {
            var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "Application/json");

            var response = await Setup.client.PostAsync("/admin/login", content);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            AdminLoggedIn adminLoggedIn = JsonSerializer.Deserialize<AdminLoggedIn>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return adminLoggedIn;
        }
    }
}
