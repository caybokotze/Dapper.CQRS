using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using static PeanutButter.RandomGenerators.RandomValueGen;

namespace Dapper.CQRS.Tests
{
    [TestFixture]
    public class CommandExecutorTests
    {
        [TestFixture]
        public class Registrations
        {
            [Test]
            public async Task AssertThatIApplicationBuilderRegistersCommandExecutor()
            {
                var hostBuilder = new HostBuilder().ConfigureWebHost(webhost =>
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
                            .ConfigureDefaults(null, DBMS.SQLite);
                    });
                });

                var host = await hostBuilder.StartAsync();
                var serviceProvider = host.Services;
                
                var commandExecutor = serviceProvider
                    .GetService<ICommandExecutor>();
                
                var actual = GetRandomInt();
                var expected = commandExecutor?
                    .Execute(new CommandInheritor(actual));
            
                Assert.AreEqual(actual, expected);
            }
            
            public class CommandInheritor : Command<int>
            {
                private readonly int _expectedReturnValue;

                public CommandInheritor(int expectedReturnValue)
                {
                    _expectedReturnValue = expectedReturnValue;
                }
                public override void Execute()
                {
                    Result = QueryFirst<int>($"Select {_expectedReturnValue};");
                }
            }
        }
    }
}