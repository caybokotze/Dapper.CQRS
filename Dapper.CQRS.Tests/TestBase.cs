using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using static PeanutButter.RandomGenerators.RandomValueGen;

namespace Dapper.CQRS.Tests
{
    [TestFixture]
    public class TestBase
    {
        public IServiceProvider ServiceProvider { get; set; }

        [SetUp]
        public async Task SetupHostEnvironment()
        {
            var hostBuilder = new HostBuilder().ConfigureWebHost(webHost =>
            {
                webHost.UseTestServer();
                webHost.Configure(app =>
                {
                    app.Run(handle => handle
                        .Response
                        .StartAsync());
                });

                webHost.ConfigureServices(config =>
                {
                    config.ConfigureDefaults(GenerateValidMySqlConnectionString(), DBMS.MySQL);
                });
            });

            var host = await hostBuilder.StartAsync();
            var serviceProvider = host.Services;

            ServiceProvider = serviceProvider;
        }
        
        public static string GenerateValidMySqlConnectionString()
        {
            return
                $"Server=localhost;Database={GetRandomString(20)};Uid={GetRandomString(20)};Pwd={GetRandomString(20)}";
        }
    }
}