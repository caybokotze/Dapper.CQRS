using System;
using System.Data;
using Dapper.CQRS.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NExpect;
using NSubstitute;
using NUnit.Framework;
using static NExpect.Expectations;
using static PeanutButter.RandomGenerators.RandomValueGen;

namespace Dapper.CQRS.Tests;

[TestFixture]
public class CommandExecutorTests
{
    [TestFixture]
    public class Registrations : TestFixtureRequiringServiceProvider
    {
        [Test]
        [Repeat(100)]
        public void AssertThatIApplicationBuilderRegistersCommandExecutor()
        {
            var commandExecutor = ServiceProvider!.GetService<ICommandExecutor>();
                
            var actual = GetRandomInt(1);
            var expected = commandExecutor!.Execute(new GenericCommand<int>(actual));
                
            Assert.AreEqual(actual, expected);
        }
    }

    [TestFixture]
    public class WhenExecutingCommand : TestFixtureRequiringServiceProvider
    {

        [Test]
        public void ShouldReturnCorrectType()
        {
            // arrange
            var dbConnection = Substitute.For<IDbConnection>();
            var queryExecutor = Substitute.For<IQueryExecutor>();
            var mockCommandExecutor = Substitute.For<ICommandExecutor>();
            var logger = Substitute.For<ILogger<SqlExecutor>>();
                
            var serviceProvider = Substitute.For<IServiceProvider>();

            serviceProvider.GetService(typeof(IDbConnection)).Returns(dbConnection);
            serviceProvider.GetService(typeof(IQueryExecutor)).Returns(queryExecutor);
            serviceProvider.GetService(typeof(ICommandExecutor)).Returns(mockCommandExecutor);
            serviceProvider.GetService(typeof(ILogger<SqlExecutor>)).Returns(logger);
                
            var commandExecutor = new CommandExecutor(serviceProvider);
                
            var command = new GenericCommand<int>(15);
            // act
            var result = commandExecutor.Execute(command);
            // assert
            Expect(result).To.Equal(15);
        }
    }
}