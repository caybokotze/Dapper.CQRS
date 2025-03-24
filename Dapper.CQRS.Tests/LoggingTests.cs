using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NExpect;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.RandomGenerators;
using static NExpect.Expectations;

namespace Dapper.CQRS.Tests;

public class LoggingTests
{
    [Test]
    public async Task ShouldLogAsExpected()
    {
        // arrange
        var genericLogger = Substitute.For<GenericLogger>();
        var hostBuilder = new HostBuilder();
        hostBuilder.ConfigureWebHost(webHost =>
        {
            webHost.ConfigureLogging(c =>
            {
                c.ClearProviders();
                c.AddConsole();
            });
            webHost.ConfigureServices(c =>
            {
                c.AddSingleton<ILogger<SqlExecutor>, GenericLogger>(s => genericLogger);
            });
            webHost.UseTestServer();
            webHost.Configure(app =>
            {
                app.Run(async ctx =>
                    await ctx
                        .Response
                        .StartAsync());
                app.Build();
            });
        });
        
        var host = await hostBuilder.StartAsync();

        // Act

        var logger = host.Services.GetRequiredService<ILogger<SqlExecutor>>();
        var logMessage = RandomValueGen.GetRandomAlphaString();

        // act
        logger.Log(LogLevel.Debug, logMessage);

        // assert
        Expect(genericLogger.Message).To.Equal(logMessage);
    }

    [TestFixture]
    public class WhenLoggingExceptionsFromWithinCommands
    {
        [Test]
        public async Task ShouldLogAsExpected()
        {
            // arrange
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(host =>
                {
                    host.UseTestServer();
                    host.ConfigureLogging(c =>
                    {
                        c.ClearProviders();
                        c.AddConsole();
                    });
                        
                    host.Configure(app =>
                    {
                        app.Run(async ctx =>
                            await ctx
                                .Response
                                .StartAsync());
                        app.Build();
                    });

                    var idbConnection = Substitute.For<IDbConnection>();

                    host.ConfigureServices(config =>
                    {
                        config.AddTransient(_ => idbConnection);
                        config.AddTransient<ICommandExecutor, CommandExecutor>();
                        config.AddTransient<IQueryExecutor, QueryExecutor>();
                    });
                });
                
            var host = await hostBuilder.StartAsync();

            // Act
                
            var commandExecutor = host.Services.GetRequiredService<ICommandExecutor>();
            var logger = host.Services.GetRequiredService<ILogger<LoggerTests>>();
            var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
            var logMessage = RandomValueGen.GetRandomAlphaString();

            // act
            commandExecutor.Execute(new LoggerTests(logMessage));
            
            // assert
            Expect(logger).Not.To.Be.Null();
            Expect(loggerFactory).Not.To.Be.Null();
        }
    }
}

public class LoggerTests : Command
{
    public readonly string LogMessage;

    public LoggerTests(string logMessage)
    {
        LogMessage = logMessage;
    }
        
    public override void Execute()
    {
        var logger = GetRequiredService<ILogger<LoggerTests>>();
        logger.LogError("Testing logger");
    }
}

public class GenericLogger : ILogger<SqlExecutor>
{
    public string? Message { get; set; }
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter)
    {
        Message = state?.ToString();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        throw new NotImplementedException();
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }
}