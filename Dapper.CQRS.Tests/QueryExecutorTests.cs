using System;
using System.Data;
using Dapper.CQRS.Tests.TestModels;
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
public class QueryExecutorTests
{
    [TestFixture]
    public class Registrations : TestFixtureRequiringServiceProvider
    {
        [Test]
        public void AssertThatIApplicationBuilderRegistersIQueryExecutor()
        {
            var queryExecutor = ServiceProvider!.GetService<IQueryExecutor>();

            var expected = GetRandomInt();
                
            var actual = queryExecutor!.Execute(new GenericQuery<int>(expected));
            
            Assert.That(expected.Equals(actual));
        }
    }
}
    
[TestFixture]
public class WhenExecutingCommand
{
    [Test]
    public void ShouldReturnCorrectType()
    {
        // arrange
        var dbConnection = Substitute.For<IDbConnection>();
        var mockableQueryExecutor = Substitute.For<IQueryExecutor>();
        var commandExecutor = Substitute.For<ICommandExecutor>();
        var logger = Substitute.For<ILogger<SqlExecutor>>();
            
        var serviceProvider = Substitute.For<IServiceProvider>();

        serviceProvider.GetService(typeof(IDbConnection)).Returns(dbConnection);
        serviceProvider.GetService(typeof(IQueryExecutor)).Returns(mockableQueryExecutor);
        serviceProvider.GetService(typeof(ICommandExecutor)).Returns(commandExecutor);
        serviceProvider.GetService(typeof(ILogger<SqlExecutor>)).Returns(logger);
            
        var user = GetRandom<User>();
        var command = new GenericQuery<User>(user);
        var queryExecutor = new QueryExecutor();
        // act
        var result = queryExecutor.Execute(command);
        // assert
        Expect(result).To.Equal(user);
    }
}