using System;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Input;
using Dapper.CQRS.Tests.Commands;
using Dapper.CQRS.Tests.Queries;
using Dapper.CQRS.Tests.TestModels;
using Dapper.CQRS.Tests.Utilities;
using Microsoft.Extensions.Logging;
using NExpect;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using static NExpect.Expectations;
using static PeanutButter.RandomGenerators.RandomValueGen;

namespace Dapper.CQRS.Tests
{
    [TestFixture]
    public class CommandTests
    {
        [TestFixture]
        public class Isolated : TestFixtureRequiringServiceProvider
        {
            [Test]
            public async Task ShouldInsertRecord()
            {
                using var scope = new TransactionScope();
                // arrange
                var commandExecutor = Resolve<ICommandExecutor>();
                var queryExecutor = Resolve<IQueryExecutor>();

                var user = new User
                {
                    Name = Faker.Name.First(),
                    Surname = Faker.Name.Last(),
                    Email = Faker.Internet.Email()
                };

                // act
                var userId = commandExecutor
                    .Execute(new GenericCommand<int>(
                        "INSERT INTO users (name, surname, email) VALUES (@Name, @Surname, @Email); SELECT LAST_INSERT_ID();", user));

                var expectedUser =
                    queryExecutor.Execute(new GenericQuery<User>("SELECT * FROM users where id = @Id",
                        new
                        {
                            Id = userId
                        }));

                if (userId.Success)
                {
                    user.Id = userId.Value;
                }
                
                // assert
                Expect(userId.Value)
                    .To
                    .Be.Greater.Than(0);

                Expect(user).To.Deep.Equal(expectedUser);
            }

            [Test]
            public async Task ShouldUpdateRecord()
            {
                using var scope = new TransactionScope();

                // arrange
                var commandExecutor = Resolve<ICommandExecutor>();
                var queryExecutor = Resolve<IQueryExecutor>();

                var user = new User
                {
                    Name = Faker.Name.First(),
                    Surname = Faker.Name.Last()
                };

                // act
                var userId = commandExecutor
                    .Execute(new GenericCommand<int>("INSERT INTO users (name, surname, email) VALUES (@Name, @Surname, @Email); SELECT LAST_INSERT_ID();", user));

                if (userId.Success)
                {
                    user.Id = userId.Value;
                }

                var expectedFirstUser = queryExecutor
                    .Execute(new GenericQuery<User>(
                        "SELECT * FROM users where id = @Id;",
                        new {Id = userId}));

                var updatedUser = new User
                {
                    Id = userId.Value,
                    Name = Faker.Name.First(),
                    Surname = Faker.Name.Last()
                };

                commandExecutor
                    .Execute(new GenericCommand<int>(
                        "UPDATE users SET name = @Name, surname = @Surname WHERE id = @Id; SELECT LAST_INSERT_ID();",
                        updatedUser));

                var expectedUpdatedUser =
                    queryExecutor.Execute(new GenericQuery<User>("SELECT * FROM users where id = @Id;",
                        new
                        {
                            Id = userId
                        }));

                // assert
                Expect(userId.Value)
                    .To
                    .Be.Greater.Than(0);

                Expect(user).To.Deep.Equal(expectedFirstUser);
                Expect(updatedUser).To.Deep.Equal(expectedUpdatedUser);
            }
            
            [TestFixture]
            public class WhenCommandReturnsPrimaryKeyValue : TestFixtureRequiringServiceProvider
            {
                [Test]
                public async Task ShouldReturnExpectedValue()
                {
                    // arrange
                    var user = GetRandom<User>();
                    var commandExecutor = Resolve<ICommandExecutor>();
                    // act
                    var sut = new InsertUser(user);
                    var result = commandExecutor.Execute(sut);
                    // assert
                    Expect(result.Value).To.Be.Greater.Than(0);
                    Expect(result.Value).To.Not.Be.Equal.To(user.Id);
                }
            }
            
            [Test]
            public void ShouldDeleteRecord()
            {
                using (new TransactionScope())
                {
                    // todo: complete...
                }
            }
        }


        // todo: complete for queries...
        [TestFixture]
        public class WhenMocking
        {
            [TestFixture]
            public class WithQueryInCommand
            {
                [Test]
                public void ShouldReturnExpectedResult()
                {
                    // arrange
                    
                    // act
                    // assert
                }
            }

            [TestFixture]
            public class WithCommandInCommand
            {
                [Test]
                public void ShouldReturnExpectedResult()
                {
                    // arrange
                    
                    // act
                    // assert
                }
            }

            [TestFixture]
            public class WithQueryInQuery
            {
                [Test]
                public void ShouldReturnExpectedResult()
                {
                    // arrange
                    
                    // act
                    // assert
                }
            }

            [TestFixture]
            public class WithCommandInQuery
            {
                [Test]
                public void ShouldReturnExpectedResult()
                {
                    // arrange
                    
                    // act
                    // assert
                }
            }
        }
        
        [TestFixture]
        public class WithQueryEmbeddedInCommand
        {
            [Test]
            public void ShouldBeMockable()
            {
                // arrange
                var serviceProvider = Substitute.For<IServiceProvider>();
                
                var queryExecutor = Substitute.For<QueryExecutor>(serviceProvider);
                var commandExecutor = Substitute.For<CommandExecutor>(serviceProvider);
                var dbConnection = Substitute.For<IDbConnection>();
                var logger = Substitute.For<ILogger<SqlExecutor>>();

                serviceProvider.GetService(typeof(IQueryExecutor)).Returns(queryExecutor);
                serviceProvider.GetService(typeof(ICommandExecutor)).Returns(commandExecutor);
                serviceProvider.GetService(typeof(IDbConnection)).Returns(dbConnection);
                serviceProvider.GetService(typeof(ILogger<SqlExecutor>)).Returns(logger);

                var user = GetRandom<User>();
                
                queryExecutor
                    .Execute(Arg.Any<FetchUser>())
                    .Returns(new SuccessResult<User>(user));

                var sut = Substitute.ForPartsOf<InsertUser>(user);

                // act
                var result = commandExecutor.Execute(sut);
                
                // assert
                Expect(sut)
                    .To.Have.Received(0)
                    .Execute(Arg.Any<string>(), Arg.Any<object>());

                Expect(result.Value).To.Equal(user.Id);
            }
        }
    }
}