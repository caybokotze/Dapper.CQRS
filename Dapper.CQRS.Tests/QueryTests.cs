using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Dapper.CQRS;
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
                    var queryable = ServiceProvider.GetRequiredService<IQueryable>();
                    var executor = ServiceProvider.GetRequiredService<IExecutable>();
                    
                    var queryExecutor = new QueryExecutor(executor, queryable, Substitute.For<ILoggerFactory>());
                    var commandExecutor = new CommandExecutor(executor, queryable, Substitute.For<ILoggerFactory>());
                    // act
                    var randomUser = GetRandom<User>();
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
                    Result = QueryList<User, UserType, UserDetails, User>(
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
                var queryable = Substitute.For<IQueryable>();
                queryable.QueryList<User>(Arg.Any<string>())
                    .Returns(new List<User>()
                    {
                        new()
                        {
                            Name = GetRandomWords(1, 2)
                        }
                    });
                queryable.QueryList<UserDetails>(Arg.Any<string>())
                    .Returns(new List<UserDetails>()
                    {
                        new()
                        {
                            IdNumber = GetRandomString()
                        }
                    });
                var executor = Substitute.For<IExecutable>();
                var queryExecutor =
                    Substitute.For<QueryExecutor>(executor, queryable, Substitute.For<ILoggerFactory>());
                var sut = new QueryUsers();
                // act
                var result = queryExecutor.Execute(sut);
                // assert
                Expect(result).To.Not.Be.Null();
                Expect(queryable).To.Have.Received(1)
                    .QueryList<User>("select * from users;");
                Expect(queryable).To.Have.Received(1)
                    .QueryList<UserDetails>("select * from user_details;");
            }
        }
    }
}

public class QueryUsers : Query<IList<User>>
{
    public override void Execute()
    {
        var userDetails = QueryExecutor.Execute(new QueryUserDetails());
        Result = QueryList<User>("select * from users;");
    }
}

public class QueryUserDetails : Query<IList<UserDetails>>
{
    public override void Execute()
    {
        Result = QueryList<UserDetails>("select * from user_details;");
    }
}