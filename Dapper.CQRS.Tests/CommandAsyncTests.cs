using System.Threading.Tasks;
using System.Transactions;
using Dapper.CQRS.Tests.TestCommands;
using Dapper.CQRS.Tests.TestModels;
using Dapper.CQRS.Tests.TestQueries;
using Dapper.CQRS.Tests.Utilities;
using NExpect;
using NUnit.Framework;
using PeanutButter.RandomGenerators;
using static NExpect.Expectations;

namespace Dapper.CQRS.Tests;

[TestFixture]
public class CommandAsyncTests
{
    [TestFixture]
    public class Isolated : TestFixtureRequiringServiceProvider
    {
        [TestFixture]
        public class WithoutReturnValue : TestFixtureRequiringServiceProvider
        {
            [Test]
            public async Task ShouldExecuteCommandAndBeRetrievable()
            {
                // arrange
                using var scope = new TransactionScope();
                var commandExecutor = Resolve<ICommandExecutor>();
                var queryExecutor = Resolve<IQueryExecutor>();
                var user = RandomValueGen.GetRandom<User>();
                var userDetails = RandomValueGen.GetRandom<UserDetails>();
                // act
                await commandExecutor.ExecuteAsync(new InsertUserAndUserDetailsAsync(user, userDetails));
                var allUsers = await queryExecutor.ExecuteAsync(new FetchAllUsersAsync());
                // assert
                Expect(allUsers.Length).To.Equal(1);
            }
        }

        [TestFixture]
        public class WithReturnValue
        {

        }

        [TestFixture]
        public class WithNestedCommand
        {

        }

        [TestFixture]
        public class WithNestedQuery
        {

        }

        [TestFixture]
        public class WhenResolvingServices
        {
            
        }

        [TestFixture]
        public class WhenExecutingParallelCommands
        {

        }
    }

    [TestFixture]
    public class Behavioural
    {
        [TestFixture]
        public class Mocking
        {
            [TestFixture]
            public class WhenMockingCommandReturnValue
            {
                
            }

            [TestFixture]
            public class WhenMockingQueryReturnValue
            {
                
            }

            [TestFixture]
            public class WhenMockingNestedCommandReturnValue
            {
                
            }

            [TestFixture]
            public class WhenLoggingInternally
            {
                
            }

            [TestFixture]
            public class WhenResolvingServices
            {
                
            }
        }
    }
}