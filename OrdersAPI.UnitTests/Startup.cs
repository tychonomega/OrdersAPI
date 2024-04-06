using dotenv.net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OrdersAPI.UnitTests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Could add services here if we like...
            //   services.AddTransient<IDependency, DependencyClass>();
        }

        public void ConfigureHost(IHostBuilder hostBuilder)
        {
            DotEnv.Load();

            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            hostBuilder.ConfigureHostConfiguration(builder => builder.AddConfiguration(config));
        }
    }
}
