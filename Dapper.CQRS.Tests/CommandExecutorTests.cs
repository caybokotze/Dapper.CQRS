using System.Data;
using Dapper.CQRS.Tests.TestModels;
using Microsoft.Extensions.DependencyInjection;
using NExpect;
using NSubstitute;
using NUnit.Framework;
using static PeanutButter.RandomGenerators.RandomValueGen;

namespace Dapper.CQRS.Tests
{
    [TestFixture]
    public class CommandExecutorTests
    {
        [TestFixture]
        public class Registrations : TestBase
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
        public class WhenExecutingCommand : TestBase
        {

            [Test]
            public void ShouldReturnCorrectType()
            {
                // arrange
                var user = GetRandom<User>();
                var command = new GenericCommand<int>(1);
                var commandExecutor = new CommandExecutor(Substitute.For<IDbConnection>());
                // act
                var result = commandExecutor.Execute(command);
                // assert
                Expectations.Expect(result).To.Equal(1);
            }
        }
        
        
    }
}