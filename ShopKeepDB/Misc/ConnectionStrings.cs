using Microsoft.Extensions.Configuration;
using System.IO;

namespace ShopKeepDB.Misc
{
    public static class ConnectionStrings
    {
        public static string GetConnectionString()
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false)
                .Build();
                return configuration.GetSection("ConnectionStrings").GetSection("ConnectionString").Value;
            }
            catch
            {
                return "";
            }
        }
    }
}
