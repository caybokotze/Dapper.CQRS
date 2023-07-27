using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Transactions;
using Dapper.CQRS.Tests.Queries;
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
                using var scope = new TransactionScope();
                // arrange
                var queryExecutor = Resolve<IQueryExecutor>();
                var commandExecutor = Resolve<ICommandExecutor>();
                // act
                var randomUser = GetRandom<User>();

                var id = commandExecutor.Execute(new GenericCommand<int>(
                    @"INSERT INTO users (name, surname, email) 
                    VALUES(@Name, @Surname, @Email); 
                    SELECT LAST_INSERT_ID();",
                    randomUser));

                var user = queryExecutor
                    .Execute(new GenericQuery<User>("SELECT * FROM users WHERE id = @Id;",
                        new
                        {
                            Id = id
                        }));
                
                // assert
                Expect(user.Value.Id).To.Equal(id.Value);
                Expect(user.Value.Name).To.Equal(randomUser.Name);
                Expect(user.Value.Surname).To.Equal(randomUser.Surname);
                Expect(user.Value.Email).To.Equal(randomUser.Email);
            }

            [TestFixture]
            public class AsyncBehaviour : TestFixtureRequiringServiceProvider
            {
                [Test]
                public async Task ShouldExecuteInSequence()
                {
                    // arrange
                    using var scope = new TransactionScope();
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var queryExecutor = Resolve<IQueryExecutor>();
                    // act
                    await queryExecutor.ExecuteAsync(new SequentialBenchmarkQuery());
                    // assert
                    stopwatch.Stop();
                    Expect(stopwatch.Elapsed.TotalMilliseconds).To.Be.Greater.Than(4000);
                }
            }

            [Test]
            public async Task ShouldResolveServiceProvider()
            {
                // arrange
                using var scope = new TransactionScope();
                var queryExecutor = Resolve<IQueryExecutor>();
                // act
                var result = queryExecutor.Execute(new ResolveDependenciesQuery());
                var expectedConnectionString = Resolve<IDbConnection>().ConnectionString;
                // assert
                Expect(result).To.Not.Be.Null();
                Expect(result.Value.ConnectionString).To.Be.Equal.To(expectedConnectionString);
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

            sut.InitialiseExecutor(serviceProvider);

            queryExecutor
                .Execute(Arg.Any<QueryUserDetails>())
                .Returns(new SuccessResult<IList<UserDetails>>(
                    new List<UserDetails>
                    {
                        new()
                        {
                            IdNumber = "123"
                        }
                    }));

            sut.QueryList<User>(Arg.Any<string>())
                .Returns(new List<User>
                {
                    GetRandom<User>()
                });

            // act
            sut.Execute();
            var result = sut.Result;
            // assert
            Expect(sut).To.Have.Received(1).QueryList<User>("select * from users;");
            Expect(queryExecutor).To.Have.Received(1).Execute(Arg.Any<QueryUserDetails>());
            Expect(result.Value.Count).To.Equal(1);
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

                sut.InitialiseExecutor(serviceProvider);
                // act
                sut.Execute();
                var result = sut.Result;
                // assert
                Expect(sut).To.Have.Received(1).QueryList<User>(Arg.Any<string>());
                Expect(queryExecutor).To.Have.Received(1).Execute(Arg.Any<QueryUserDetails>());
                Expect(result.Value).To.Deep.Equal(new List<User>());
            }
        }
    }
    
    public class QueryUsers : Query<IList<User>>
    {
        public override void Execute()
        {
            var userDetails = QueryExecutor.Execute(new QueryUserDetails());
            var users =  QueryList<User>("select * from users;");

            Result = new SuccessResult<IList<User>>(users.ToList());
        }
    }

    public class QueryUserDetails : Query<IList<UserDetails>>
    {
        public override void Execute()
        {
            var result = QueryList<UserDetails>("select * from user_details;");

            Result = new SuccessResult<IList<UserDetails>>(result.ToList());
        }
    }
}


