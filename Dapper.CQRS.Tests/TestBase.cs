using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using static ScopeFunction.Utils.AppSettingsBuilder;

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
                DefaultTypeMap.MatchNamesWithUnderscores = true;
                webHost.UseTestServer();
                webHost.Configure(app =>
                {
                    app.Run(handle => handle
                        .Response
                        .StartAsync());
                });

                webHost.ConfigureServices(config =>
                {
                    config.AddTransient<ICommandExecutor, CommandExecutor>();
                    config.AddTransient<IQueryExecutor, QueryExecutor>();
                    config.AddTransient<IDbConnection, MySqlConnection>(
                        s => new MySqlConnection(GetConnectionString()));
                });
            });

            var host = await hostBuilder.StartAsync();
            var serviceProvider = host.Services;

            ServiceProvider = serviceProvider;
        }

        private string GetConnectionString()
        {
            var configurationRoot = CreateConfigurationRoot();
            return configurationRoot
                .GetConnectionString("DefaultConnection");
        }
    }
}