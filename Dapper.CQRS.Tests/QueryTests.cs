using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using Dapper.CQRS.Tests.TestModels;
using Dapper.CQRS.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NExpect;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.RandomGenerators;
using static NExpect.Expectations;

namespace Dapper.CQRS.Tests
{
    [TestFixture]
    public class QueryTests
    {
        [TestFixture]
        public class SelectionTests : TestFixtureRequiringServiceProvider
        {
            [Test]
            public void ShouldSelectAndReturnUser()
            {
                using (new TransactionScope())
                {
                    // arrange
                    var queryExecutor = new QueryExecutor(ServiceProvider.GetRequiredService<IDbConnection>(), Substitute.For<ILogger<BaseSqlExecutor>>());
                    var commandExecutor = new CommandExecutor(ServiceProvider.GetRequiredService<IDbConnection>(), Substitute.For<ILogger<BaseSqlExecutor>>());
                    // act
                    var randomUser = RandomValueGen.GetRandom<User>();
                    var id = commandExecutor.Execute(
                        new GenericCommand<int>(@"INSERT INTO users (name, surname, email) 
                    VALUES(@Name, @Surname, @Email); SELECT LAST_INSERT_ID();", randomUser));
                
                    var user = queryExecutor
                        .Execute(new GenericQuery<User>("SELECT * FROM users WHERE id = @Id;",
                            new {
                                Id = id
                            }));
                    // assert
                    Expect(user.Id).To.Equal(id);
                    Expect(user.Name).To.Equal(randomUser.Name);
                    Expect(user.Surname).To.Equal(randomUser.Surname);
                    Expect(user.Email).To.Equal(randomUser.Email);
                }
            }
        }

        [TestFixture]
        public class GenericParameterTests
        {
            public class QueryWith4GenericParameters : Query<List<User>>
            {
                public override void Execute()
                {
                    Result = Query<User, UserType, UserDetails, User>(
                        "SELECT * FROM users LEFT JOIN user_type ON users.id = user_type.user_id;",
                        (user, type, details) =>
                        {
                            user.UserType = type;
                            user.UserDetails = details;
                            return user;
                        })
                        .ToList();
                }
            }
            
            [Test]
            public void ShouldMerge4GenericParameters()
            {
                using (new TransactionScope())
                {
                    // arrange
                    
                    // act
                    // assert
                }
            }
        }

        [TestFixture]
        public class WithQueriesEmbeddedInQueries
        {
            [Test]
            public void ShouldBeMockable()
            {
                // arrange
                
                // act
                // assert
            }
        }
    }
}