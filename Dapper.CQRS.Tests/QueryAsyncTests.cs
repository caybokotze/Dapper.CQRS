using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Transactions;
using Dapper.CQRS.Tests.Commands;
using Dapper.CQRS.Tests.Queries;
using Dapper.CQRS.Tests.TestModels;
using Dapper.CQRS.Tests.Utilities;
using NExpect;
using NUnit.Framework;
using static NExpect.Expectations;
using static PeanutButter.RandomGenerators.RandomValueGen;

namespace Dapper.CQRS.Tests;

[TestFixture]
public class QueryAsyncTests
{
    [TestFixture]
    public class Isolated
    {
        [TestFixture]
        public class WhenFetchingUserAndUserDetailsInSingleQuery : TestFixtureRequiringServiceProvider
        {
            [Repeat(10)]
            [Test]
            public async Task ShouldReturnExpectedResultAndRollback()
            {
                // arrange
                using var scope = new TransactionScope();
                var commandExecutor = Resolve<ICommandExecutor>();
                var queryExecutor = Resolve<IQueryExecutor>();
                
                var user = GetRandom<User>();
                var userDetail = GetRandom<UserDetail>();
                // act
                var userId = commandExecutor.Execute(new InsertUser(user));
                user.Id = userId;
                userDetail.UserId = userId;
                
                var userDetailId = commandExecutor.Execute(new InsertUserDetail(userDetail));
                userDetail.Id = userDetailId;

                var sut = new FetchUserAndDetailsById(userId);
                var result = await queryExecutor.ExecuteAsync(sut);
                // assert
                Expect(result!.Id).To.Equal(userId);
                Expect(result.Email).To.Equal(user.Email);
                Expect(result.Name).To.Equal(user.Name);
                Expect(result.Surname).To.Equal(user.Surname);
                Expect(result.UserDetails).To.Deep.Equal(userDetail);
                scope.Dispose();
                
                var userAfterDispose = await queryExecutor.ExecuteAsync(new FetchUserById(userId));
                Expect(userAfterDispose).To.Be.Null();
            }
        }

        [TestFixture]
        public class WhenFetchingUserAndUserDetailsInEmbeddedQuery : TestFixtureRequiringServiceProvider
        {
            [Repeat(10)]
            [Test]
            public async Task ShouldReturnExpectedResultAndRollback()
            {
                // arrange
                using var scope = new TransactionScope();
                var commandExecutor = Resolve<ICommandExecutor>();
                var queryExecutor = Resolve<IQueryExecutor>();

                var user = GetRandom<User>();
                var userDetail = GetRandom<UserDetail>();
                
                // act
                var userId = commandExecutor.Execute(new InsertUser(user));
                user.Id = userId;
                userDetail.UserId = userId;

                var userDetailId = commandExecutor.Execute(new InsertUserDetail(userDetail));
                userDetail.Id = userDetailId;

                var sut = new FetchUserAndDetailsByIdAsEmbeddedQuery(userId);
                var result = await queryExecutor.ExecuteAsync(sut);
                
                // assert
                Expect(result!.Id).To.Equal(userId);
                Expect(result.Email).To.Equal(user.Email);
                Expect(result.Name).To.Equal(user.Name);
                Expect(result.Surname).To.Equal(user.Surname);
                Expect(result.UserDetails).To.Deep.Equal(userDetail);
                scope.Dispose();
                
                var userAfterDispose = await queryExecutor.ExecuteAsync(new FetchUserById(userId));
                Expect(userAfterDispose).To.Be.Null();
            }    
        }


        [TestFixture]
        public class WhenFetchingUserAndUserDetailsInSeparateQuery : TestFixtureRequiringServiceProvider
        {
            [Repeat(10)]
            [Test]
            public async Task ShouldReturnExpectedResultAndRollback()
            {
                // arrange
                using var scope = new TransactionScope();
                var timer = new Stopwatch();
                timer.Start();
                var commandExecutor = Resolve<ICommandExecutor>();
                var queryExecutor = Resolve<IQueryExecutor>();

                var user = GetRandom<User>();
                var userDetail = GetRandom<UserDetail>();
                
                // act
                var userId = commandExecutor.Execute(new InsertUser(user));
                user.Id = userId;
                userDetail.UserId = userId;

                var userDetailId = commandExecutor.Execute(new InsertUserDetail(userDetail));
                userDetail.Id = userDetailId;

                var userResult = await queryExecutor.ExecuteAsync(new FetchUserById(userId));
                var userDetailResult = await queryExecutor.ExecuteAsync(new FetchUserDetailsByUserId(userId));
                
                // assert
                Expect(userResult!.Id).To.Equal(userId);
                Expect(userResult.Email).To.Equal(user.Email);
                Expect(userResult.Name).To.Equal(user.Name);
                Expect(userResult.Surname).To.Equal(user.Surname);
                Expect(userDetailResult!.Id).To.Equal(userDetailId);
                Expect(userDetailResult.IdNumber).To.Equal(userDetail.IdNumber);
                
                scope.Dispose();
                var userAfterDispose = await queryExecutor.ExecuteAsync(new FetchUserById(userId));
                Expect(userAfterDispose).To.Be.Null();
                timer.Stop();
                var timeTaken = timer.Elapsed;
                Console.WriteLine("The thing took {0} milliseconds", timeTaken.TotalMilliseconds);
            }
        }

        [TestFixture]
        public class WhenUserAndUserDetailsInSeparateQueryConcurrently : TestFixtureRequiringServiceProvider
        {
            [Repeat(10)]
            [Test]
            public async Task ShouldReturnExpectedResultAndRollback()
            {
                // arrange
                using var scope = new TransactionScope();
                var timer = new Stopwatch();
                timer.Start();
                var commandExecutor = Resolve<ICommandExecutor>();
                var queryExecutor = Resolve<IQueryExecutor>();

                var user = GetRandom<User>();
                var userDetail = GetRandom<UserDetail>();
                
                // act
                var userId = commandExecutor.Execute(new InsertUser(user));
                user.Id = userId;
                userDetail.UserId = userId;

                var userDetailId = commandExecutor.Execute(new InsertUserDetail(userDetail));
                userDetail.Id = userDetailId;

                var userResultTask = queryExecutor.ExecuteAsync(new FetchUserById(userId));
                var userDetailResultTask = queryExecutor.ExecuteAsync(new FetchUserDetailsByUserId(userId));

                await Task.WhenAll(userResultTask, userDetailResultTask);
                var userResult = userResultTask.Result;
                var userDetailResult = userDetailResultTask.Result;
                
                // assert
                Expect(userResult!.Id).To.Equal(userId);
                Expect(userResult.Email).To.Equal(user.Email);
                Expect(userResult.Name).To.Equal(user.Name);
                Expect(userResult.Surname).To.Equal(user.Surname);
                Expect(userDetailResult!.Id).To.Equal(userDetailId);
                Expect(userDetailResult.IdNumber).To.Equal(userDetail.IdNumber);
                
                scope.Dispose();
                var userAfterDispose = await queryExecutor.ExecuteAsync(new FetchUserById(userId));
                Expect(userAfterDispose).To.Be.Null();
                
                timer.Stop();
                var timeTaken = timer.Elapsed;
                Console.WriteLine("The thing took {0} milliseconds", timeTaken.TotalMilliseconds);
            }
        }
    }
}