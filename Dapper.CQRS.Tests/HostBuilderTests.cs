using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace Dapper.CQRS.Tests;

public class HostBuilderTests
{
    [Test]
    public async Task AssertThatHostBuilderRegistersSuccessfully()
    {
        // Arrange
        var expected = "HostBuilder was started successfully.";
            
        var hostBuilder = new HostBuilder()
            .ConfigureWebHost(webhost =>
            {
                webhost.UseTestServer();
                webhost.Configure(app => app
                    .Run(async ctx => 
                        await ctx
                            .Response
                            .WriteAsync(expected)));
            });

        // Act
        var host = await hostBuilder.StartAsync();

        var client = host.GetTestClient();
        var response = await client.GetAsync("/");

        response.EnsureSuccessStatusCode();
        var actual = await response.Content.ReadAsStringAsync();
            
        // Assert
        Assert.That(expected.Equals(actual));
    }
}