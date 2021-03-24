using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace DapperDoodle.Tests
{
    public class HostBuilder
    {
        public static async Task<IServiceProvider> CreateHostBuilder()
        {
            var hostBuilder = new Microsoft.Extensions.Hosting.HostBuilder().ConfigureWebHost(webhost =>
            {
                webhost.UseTestServer();
                webhost.Configure(app =>
                {
                    app.Run(handle => handle
                        .Response
                        .StartAsync());
                });

                webhost.ConfigureServices(config =>
                {
                    config
                        .ConfigureDapperDoodle(null, DBMS.SQLite);
                });
            });
            
            var host = await hostBuilder.StartAsync();
            return host.Services;
        }
    }
}