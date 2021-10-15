using System.Data;
using Dapper.CQRS.Tests.TestModels;
using Microsoft.Extensions.DependencyInjection;
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
        public class Registrations : TestBase
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
    public class WhenExecutingCommand : TestBase
    {

        [Test]
        public void ShouldReturnCorrectType()
        {
            // arrange
            var user = GetRandom<User>();
            var command = new GenericQuery<User>(user);
            var commandExecutor = new QueryExecutor(Substitute.For<IDbConnection>());
            // act
            var result = commandExecutor.Execute(command);
            // assert
            Expect(result).To.Equal(user);
        }
    }
}