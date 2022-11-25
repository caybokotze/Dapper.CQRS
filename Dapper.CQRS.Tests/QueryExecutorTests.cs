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

namespace Dapper.CQRS.Tests
{
    [TestFixture]
    public class QueryExecutorTests
    {
        [TestFixture]
        public class Registrations : TestFixtureRequiringServiceProvider
        {
            [Test]
            public void AssertThatIApplicationBuilderRegistersIQueryExecutor()
            {
                var queryExecutor = ServiceProvider
                    .GetService<IQueryExecutor>();

                var actual = GetRandomInt();
                
                var expected = queryExecutor?
                    .Execute(new GenericQuery<int>(actual));
            
                Assert.That(expected.Equals(actual));
            }
        }
    }
    
    [TestFixture]
    public class WhenExecutingCommand : TestFixtureRequiringServiceProvider
    {

        [Test]
        public void ShouldReturnCorrectType()
        {
            // arrange
            var serviceProvider = Substitute.For<IServiceProvider>();
            
            var user = GetRandom<User>();
            var command = new GenericQuery<User>(user);
            var queryExecutor = new QueryExecutor(serviceProvider);
            // act
            var result = queryExecutor.Execute(command);
            // assert
            Expect(result).To.Equal(user);
        }
    }
}