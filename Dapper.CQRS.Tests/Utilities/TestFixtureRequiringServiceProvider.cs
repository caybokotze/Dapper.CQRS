using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Dapper.CQRS.Tests.Utilities;

[TestFixture]
public class TestFixtureRequiringServiceProvider
{
    public IServiceProvider? ServiceProvider { get; set; }

    public T Resolve<T>() where T : notnull
    {
        if (ServiceProvider is null)
        {
            throw new NullReferenceException("The service provider has not been configured correctly");
        }
            
        return ServiceProvider.GetRequiredService<T>();
    }

    [SetUp]
    public async Task SetupHostEnvironment()
    {
        var hostBuilder = new HostBuilder()
            .ConfigureWebHost(webHost =>
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
            });
        });
        
        hostBuilder.ConfigureLogging(l =>
        {
            l.ClearProviders();
            l.AddConsole();
        });

        var host = await hostBuilder.StartAsync();
        var serviceProvider = host.Services;
        
        new CqrsConfigurationBuilder()
            .WithDefaultQueryTimeout(5)
            .WithConnectionFactory(new ConnectionFactory(GetConnectionString()))
            .WithSnakeCaseMappingsEnabled()
            .WithServiceProvider(serviceProvider);

        ServiceProvider = serviceProvider;
    }

    private string GetConnectionString()
    {
        var configurationRoot = CreateConfigurationRoot();
        return configurationRoot.GetConnectionString("DefaultConnection")!;
    }
        
    private static IConfigurationRoot CreateConfigurationRoot()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            
        if (CanLoad("appsettings.deploy.json"))
            builder.AddJsonFile("appsettings.deploy.json");
            
        return builder.Build();
    }

    private static bool CanLoad(string deployConfig)
    {
        if (!File.Exists(deployConfig))
            return false;
        var source = File.ReadAllLines(deployConfig);
        var re = new Regex("#{.+}");
        return source.All(l => !re.Match(l).Success);
    }
}