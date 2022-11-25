using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using Dapper.CQRS;
using Dapper.CQRS.Tests.TestModels;
using Dapper.CQRS.Tests.Utilities;
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
        public class Isolated : TestFixtureRequiringServiceProvider
        {
            [Test]
            public void ShouldSelectAndReturnUser()
            {
                using (new TransactionScope())
                {
                    // arrange
                    var dbConnection = Substitute.For<IDbConnection>();
                    var queryExecutor = new QueryExecutor(null, dbConnection, Substitute.For<ILoggerFactory>());
                    var commandExecutor = new CommandExecutor(queryExecutor, dbConnection, Substitute.For<ILoggerFactory>());
                    // act
                    var randomUser = GetRandom<User>();
                    
                    var id = commandExecutor.Execute(new GenericCommand<int>(@"INSERT INTO users (name, surname, email) 
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
            public class QueryWith4GenericParameters : Query<IList<User>>
            {
                public override IList<User> Execute()
                {
                    return QueryList<User, UserType, UserDetails, User>(
                        "SELECT * FROM users LEFT JOIN user_type ON users.id = user_type.user_id;",
                        (user, type, details) =>
                        {
                            user.UserType = type;
                            user.UserDetails = details;
                            return user;
                        });
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
        public class WhenMocking
        {
            [TestFixture]
            public class WhenQueryInsideOfQuery
            {
                
            }

            [TestFixture]
            public class WhenCommandInsideOfQuery
            {
                
            }

            [TestFixture]
            public class WhenCommandAndQueryInsideQuery
            {
                
            }
        }

        [TestFixture]
        public class WithQueriesEmbeddedInQueries
        {
            [Test]
            public void ShouldBeMockable()
            {
                // arrange
                var loggerFactory = Substitute.For<ILoggerFactory>();
                var dbConnection = Substitute.For<IDbConnection>();
                var commandExecutor = Substitute.For<ICommandExecutor>();
                var queryExecutor = Substitute.For<IQueryExecutor>();
                
                var sut = Substitute.ForPartsOf<QueryUsers>();
                
                sut.Initialise(queryExecutor, commandExecutor, dbConnection, loggerFactory);
                
                queryExecutor.Execute(Arg.Any<QueryUserDetails>())
                    .Returns(new List<UserDetails>
                    {
                        new()
                        {
                            IdNumber = "123"
                        }
                    });

                sut.QueryList<User>(Arg.Any<string>())
                    .Returns(new List<User>{ new User() });
                
                // act
                var result = sut.Execute();
                // assert
                Expect(sut).To.Have.Received(1).QueryList<User>(Arg.Any<string>());
                Expect(queryExecutor).To.Have.Received(1).Execute(Arg.Any<QueryUserDetails>());
            }

            [TestFixture]
            public class WithoutDefiningReturnValue
            {
                [Test]
                public void ShouldRecordMockedCalls()
                {
                    // arrange
                    var queryExecutor = Substitute.For<IQueryExecutor>();
                    var serviceProvider = Substitute.For<IServiceProvider>();
                    serviceProvider.GetService(typeof(IQueryExecutor)).Returns(queryExecutor);
                
                    var sut = Substitute.ForPartsOf<QueryUsers>();
                
                    sut.Initialise(serviceProvider);
                    // act
                    var result = sut.Execute();
                    // assert
                    Expect(sut).To.Have.Received(1).QueryList<User>(Arg.Any<string>());
                    Expect(queryExecutor).To.Have.Received(1).Execute(Arg.Any<QueryUserDetails>());
                    Expect(result).To.Deep.Equal(new List<User>());
                }
            }
        }
    }
}

public class QueryUsers : Query<IList<User>>
{
    public override IList<User> Execute()
    {
        var userDetails = QueryExecutor.Execute(new QueryUserDetails());
        return QueryList<User>("select * from users;");
    }
}

public class QueryUserDetails : Query<IList<UserDetails>>
{
    public override IList<UserDetails> Execute()
    {
        return QueryList<UserDetails>("select * from user_details;");
    }
}