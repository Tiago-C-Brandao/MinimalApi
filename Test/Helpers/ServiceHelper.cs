using Microsoft.Extensions.Configuration;
using MinimalApi.Infrastructure.Db;
using System.Reflection;

namespace Test.Utils
{
    public static class ServiceHelper
    {
        public static MinimalApiContext CreateTestContext()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            return new MinimalApiContext(config);
        }
    }
}
