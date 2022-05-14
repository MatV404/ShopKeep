using System.IO;
using Microsoft.Extensions.Configuration;

namespace ShopKeepDB.Misc
{
    public static class ConnectionStrings
    {
        public static string GetConnectionString(string name)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false)
                .Build();
            return configuration.GetSection("ConnectionStrings").GetSection("ConnectionString").Value;
        }
    }
}
