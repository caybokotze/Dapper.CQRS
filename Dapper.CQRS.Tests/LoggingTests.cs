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

namespace Dapper.CQRS.Tests
{
    public class LoggingTests
    {
        [Test]
        public async Task ShouldLogAsExpected()
        {
            // arrange
            var gl = new GenericLogger();
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webhost =>
                {
                    webhost.UseTestServer();
                    webhost.Configure(app =>
                    {
                        app.Run(async ctx =>
                            await ctx
                                .Response
                                .StartAsync());
                        app.Build();
                    });

                    webhost.ConfigureServices(config =>
                    {
                        config.AddSingleton<ILogger<BaseSqlExecutor>, GenericLogger>(_ => gl);
                    });
                });
            var host = await hostBuilder.StartAsync();
            
            // Act

            var logger = host.Services.GetRequiredService<ILogger<BaseSqlExecutor>>();
            var logMessage = RandomValueGen.GetRandomAlphaString();

            // act
            logger.Log(LogLevel.Debug, logMessage);
            
            // assert
            Expect(gl.LogState).To.Equal(logMessage);
        }

        [TestFixture]
        public class WhenLoggingExceptionsFromWithinCommands
        {
            [Test]
            public async Task ShouldLogAsExpected()
            {
                // arrange
                var gl = new GenericLogger();
                var hostBuilder = new HostBuilder()
                    .ConfigureWebHost(host =>
                    {
                        host.UseTestServer();
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
                            config.AddTransient<IExecutable, Executable>();
                            config.AddTransient<IQueryable, Queryable>();
                            config.AddTransient<ICommandExecutor, CommandExecutor>();
                            config.AddSingleton<ILogger<BaseSqlExecutor>, GenericLogger>(_ => gl);
                        });
                    });
                
                var host = await hostBuilder.StartAsync();

                // Act
                
                var commandExecutor = host.Services.GetRequiredService<ICommandExecutor>();
                var logMessage = RandomValueGen.GetRandomAlphaString();

                // act
                commandExecutor.Execute(new LoggerTests(logMessage));
            
                // assert
                Expect(gl.LogState).To.Equal(logMessage);
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
            Logger.Log(LogLevel.Debug, LogMessage);
        }
    }

    public class GenericLogger : ILogger<BaseSqlExecutor>
    {
        public string LogState { get; set; }
        
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            LogState = state.ToString();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            throw new NotImplementedException();
        }
    }
}