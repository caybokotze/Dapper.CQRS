using System;
using System.Data;
using System.Transactions;
using Dapper.CQRS.Tests.Commands;
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
    public class CommandTests
    {
        [TestFixture]
        public class Transactions : TestFixtureRequiringServiceProvider
        {
            [Test]
            public void ShouldInsertRecord()
            {
                using (new TransactionScope())
                {
                    // arrange
                    var serviceProvider = Substitute.For<IServiceProvider>();
                    var commandExecutor = new CommandExecutor(serviceProvider);
                    var queryExecutor = new QueryExecutor(serviceProvider);
                    
                    var user = new User
                    {
                        Name = Faker.Name.First(),
                        Surname = Faker.Name.Last(),
                        Email = Faker.Internet.Email()
                    };
                    
                    // act
                    var userId = commandExecutor
                        .Execute(new GenericCommand<int>(
                            "INSERT INTO users (name, surname, email) VALUES (@Name, @Surname, @Email); SELECT LAST_INSERT_ID();",
                            user));

                    var expectedUser =
                        queryExecutor.Execute(new GenericQuery<User>("SELECT * FROM users where id = @Id",
                            new {Id = userId}));

                    user.Id = userId;
                    
                    // assert
                    Expect(userId)
                        .To
                        .Be.Greater.Than(0);
                    
                    Expect(user).To.Deep.Equal(expectedUser);
                }
            }

            [Test]
            public void ShouldUpdateRecord()
            {
                using (new TransactionScope())
                {
                    // arrange
                    var serviceProvider = Substitute.For<IServiceProvider>();
                    var commandExecutor = new CommandExecutor(serviceProvider);
                    var queryExecutor = new QueryExecutor(serviceProvider);

                    var user = new User
                    {
                        Name = Faker.Name.First(),
                        Surname = Faker.Name.Last()
                    };
                    
                    // act
                    var userId = commandExecutor
                        .Execute(new GenericCommand<int>(
                            "INSERT INTO users (name, surname, email) VALUES (@Name, @Surname, @Email); SELECT LAST_INSERT_ID();",
                            user));

                    user.Id = userId;
                    
                    var expectedFirstUser =
                        queryExecutor.Execute(new GenericQuery<User>("SELECT * FROM users where id = @Id;",
                            new { Id = userId }));

                    var updatedUser = new User
                    {
                        Id = userId,
                        Name = Faker.Name.First(),
                        Surname = Faker.Name.Last()
                    };

                    commandExecutor
                        .Execute(new GenericCommand<int>("UPDATE users SET name = @Name, surname = @Surname WHERE id = @Id; SELECT LAST_INSERT_ID();", updatedUser));

                    var expectedUpdatedUser =
                        queryExecutor.Execute(new GenericQuery<User>("SELECT * FROM users where id = @Id;",
                            new { Id = userId }));

                    // assert
                    Expect(userId)
                        .To
                        .Be.Greater.Than(0);
                    
                    Expect(user).To.Deep.Equal(expectedFirstUser);
                    Expect(updatedUser).To.Deep.Equal(expectedUpdatedUser);
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
                var queryExecutor = Substitute.For<IQueryExecutor>();
                var dbConnection = Substitute.For<IDbConnection>();
                
                var commandExecutor =
                    Substitute.For<CommandExecutor>(queryExecutor, dbConnection, Substitute.For<ILoggerFactory>());
                
                var user = GetRandom<User>();
                queryExecutor.Execute(Arg.Any<FetchUser>()).Returns(user);
                
                var sut = new InsertUser(user);
                // act
                commandExecutor.Execute(sut);
                // assert
            }
        }
    }
}