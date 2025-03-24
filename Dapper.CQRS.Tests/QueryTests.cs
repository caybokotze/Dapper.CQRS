using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

namespace Dapper.CQRS.Tests;

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
            Expect(user.Id).To.Equal(id);
            Expect(user.Name).To.Equal(randomUser.Name);
            Expect(user.Surname).To.Equal(randomUser.Surname);
            Expect(user.Email).To.Equal(randomUser.Email);
        }

        [TestFixture]
        public class AsyncBehaviour : TestFixtureRequiringServiceProvider
        {
            [Test]
            public async Task ShouldExecuteInSequence()
            {
                // arrange
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var queryExecutor = Resolve<IQueryExecutor>();
                // act
                await queryExecutor.ExecuteAsync(new SequentialBenchmarkQuery());
                // assert
                stopwatch.Stop();
                var totalSequential = stopwatch.Elapsed.TotalMilliseconds;
                stopwatch.Reset();
                stopwatch.Start();
                await queryExecutor.ExecuteAsync(new ParallelBenchmarkQuery());
                stopwatch.Stop();
                var totalParallel = stopwatch.Elapsed.TotalMilliseconds;
                Expect(totalParallel).To.Be.Less.Than(totalSequential / 2);
            }

            [Test]
            public async Task ShouldExecuteAndReturnUserAndDetails()
            {
                // arrange
                var queryExecutor = Resolve<IQueryExecutor>();
                var commandExecutor = Resolve<ICommandExecutor>();
                    
                var randomUser = GetRandom<User>();

                var userIdResult = commandExecutor.Execute(new GenericCommand<int>(
                    @"
                                INSERT INTO users (name, surname, email) 
                                VALUES(@Name, @Surname, @Email); 
                                SELECT LAST_INSERT_ID();",
                    randomUser));

                var randomDetails = GetRandom<UserDetail>();
                randomDetails.UserId = userIdResult;
                    
                var userDetailsIdResult = commandExecutor.Execute(new GenericCommand<int>(
                    @"
                                INSERT INTO user_details (user_id, id_number) 
                                VALUES(@UserId, @IdNumber); 
                                SELECT LAST_INSERT_ID();",
                    randomDetails));
                    
                // assert
                var userDetails = await queryExecutor.ExecuteAsync(new ParallelUserDetailsQuery(userIdResult));

                Expect(userDetails.Id).To.Equal(userIdResult);
                Expect(userDetails.Email).To.Equal(randomUser.Email);
                Expect(userDetails.Name).To.Equal(randomUser.Name);
                Expect(userDetails.Surname).To.Equal(randomUser.Surname);
                Expect(userDetails.UserDetails!.IdNumber).To.Equal(randomDetails.IdNumber);
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
            Expect(result.ConnectionString).To.Be.Equal.To(expectedConnectionString);
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
            
        var mockDbConnection = Substitute.For<IDbConnection>();
        var mockQueryExecutor = Substitute.For<IQueryExecutor>();

        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider.GetService(typeof(IDbConnection)).Returns(mockDbConnection);
        serviceProvider.GetService(typeof(ILogger<SqlExecutor>)).Returns(logger);
            
        var sut = Substitute.ForPartsOf<QueryUsers>();

        mockQueryExecutor.Execute(Arg.Any<QueryUserDetails>())
            .Returns(new List<UserDetail>
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
        sut.Execute();
        // assert
        Expect(sut).To.Have.Received(1).QueryList<User>("select * from users;");
        Expect(mockQueryExecutor).To.Have.Received(1).Execute(Arg.Any<QueryUserDetails>());
    }
}
    
public class QueryUsers : Query<IList<User>>
{
    public override IList<User> Execute()
    {
        var userDetails = QueryExecutor.Execute(new QueryUserDetails());
        var users =  QueryList<User>("select * from users;");

        return users.ToList();
    }
}

public class QueryUserDetails : Query<IList<UserDetail>>
{
    public override IList<UserDetail> Execute()
    {
        var result = QueryList<UserDetail>("select * from user_details;");

        return result.ToList();
    }
}