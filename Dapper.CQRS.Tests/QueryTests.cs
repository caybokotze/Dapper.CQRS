using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;
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
            public async Task ShouldSelectAndReturnUser()
            {
                using var scope = new TransactionScope();
                // arrange
                var queryExecutor = Resolve<IQueryExecutor>();
                var commandExecutor = Resolve<ICommandExecutor>();
                // act
                var randomUser = GetRandom<User>();

                var id = await commandExecutor.Execute(new GenericCommand<int>(
                    @"INSERT INTO users (name, surname, email) 
                    VALUES(@Name, @Surname, @Email); 
                    SELECT LAST_INSERT_ID();",
                    randomUser));

                var user = await queryExecutor
                    .Execute(new GenericQuery<User>("SELECT * FROM users WHERE id = @Id;",
                        new
                        {
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
        // todo: complete tests...
        public class QueryWith4GenericParameters : Query<IList<User>>
        {
            public override Task<IList<User>> Execute()
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
        public async Task ShouldBeMockable()
        {
            // arrange
            var logger = Substitute.For<ILogger<SqlExecutor>>();
            var dbConnection = Substitute.For<IDbConnection>();
            var commandExecutor = Substitute.For<ICommandExecutor>();
            var queryExecutor = Substitute.For<IQueryExecutor>();

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(ICommandExecutor)).Returns(commandExecutor);
            serviceProvider.GetService(typeof(IQueryExecutor)).Returns(queryExecutor);
            serviceProvider.GetService(typeof(IDbConnection)).Returns(dbConnection);
            serviceProvider.GetService(typeof(ILogger<SqlExecutor>)).Returns(logger);

            var sut = Substitute.ForPartsOf<QueryUsers>();

            sut.Initialise(serviceProvider);

            queryExecutor
                .Execute(Arg.Any<QueryUserDetails>())
                .Returns(new List<UserDetails>
                {
                    new()
                    {
                        IdNumber = "123"
                    }
                });

            sut.QueryList<User>(Arg.Any<string>())
                .Returns(new List<User>
                {
                    GetRandom<User>()
                });

            // act
            var result = await sut.Execute();
            // assert
            await Expect(sut).To.Have.Received(1).QueryList<User>("select * from users;");
            await Expect(queryExecutor).To.Have.Received(1).Execute(Arg.Any<QueryUserDetails>());
            Expect(result.Count).To.Equal(1);
        }

        [TestFixture]
        public class WithoutDefiningReturnValue
        {
            [Test]
            public async Task ShouldRecordMockedCalls()
            {
                // arrange
                var queryExecutor = Substitute.For<IQueryExecutor>();
                var commandExecutor = Substitute.For<ICommandExecutor>();
                var dbConnection = Substitute.For<IDbConnection>();
                var logger = Substitute.For<ILogger<SqlExecutor>>();
                
                var serviceProvider = Substitute.For<IServiceProvider>();
                
                serviceProvider.GetService(typeof(IQueryExecutor)).Returns(queryExecutor);
                serviceProvider.GetService(typeof(ICommandExecutor)).Returns(commandExecutor);
                serviceProvider.GetService(typeof(IDbConnection)).Returns(dbConnection);
                serviceProvider.GetService(typeof(ILogger<SqlExecutor>)).Returns(logger);

                var sut = Substitute.ForPartsOf<QueryUsers>();

                sut.Initialise(serviceProvider);
                // act
                var result = await sut.Execute();
                // assert
                await Expect(sut).To.Have.Received(1).QueryList<User>(Arg.Any<string>());
                await Expect(queryExecutor).To.Have.Received(1).Execute(Arg.Any<QueryUserDetails>());
                Expect(result).To.Deep.Equal(new List<User>());
            }
        }
    }
    
    public class QueryUsers : Query<IList<User>>
    {
        public override Task<IList<User>> Execute()
        {
            var userDetails = QueryExecutor.Execute(new QueryUserDetails());
            return QueryList<User>("select * from users;");
        }
    }

    public class QueryUserDetails : Query<IList<UserDetails>>
    {
        public override Task<IList<UserDetails>> Execute()
        {
            return QueryList<UserDetails>("select * from user_details;");
        }
    }
}


