using System.Data;
using Dapper.CQRS.Tests.TestModels;
using Dapper.CQRS.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NExpect;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using static PeanutButter.RandomGenerators.RandomValueGen;

namespace Dapper.CQRS.Tests
{
    [TestFixture]
    public class CommandExecutorTests
    {
        [TestFixture]
        public class Registrations : TestFixtureRequiringServiceProvider
        {
            [Test]
            public void AssertThatIApplicationBuilderRegistersCommandExecutor()
            {
                var commandExecutor = ServiceProvider
                    .GetService<ICommandExecutor>();
                
                var actual = GetRandomInt();
                var expected = commandExecutor?
                    .Execute(new GenericCommand<int>(actual));
            
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
                var user = GetRandom<User>();
                var command = new GenericCommand<int>(1);
                var commandExecutor = new CommandExecutor(
                    Substitute.For<IExecutor>(), 
                    Substitute.For<IQueryable>(), 
                    Substitute.For<ILogger<BaseSqlExecutor>>());
                // act
                var result = commandExecutor.Execute(command);
                // assert
                Expectations.Expect(result).To.Equal(1);
            }
        }
        
        
    }
}