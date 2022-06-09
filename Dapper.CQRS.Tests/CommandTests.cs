using System.Data;
using System.Transactions;
using Dapper.CQRS.Tests.Commands;
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
                    var commandExecutor = new CommandExecutor(ServiceProvider
                        .GetRequiredService<IDbConnection>(), Substitute.For<ILogger<BaseSqlExecutor>>());

                    var queryExecutor = new QueryExecutor(ServiceProvider
                        .GetRequiredService<IDbConnection>(), Substitute.For<ILogger<BaseSqlExecutor>>());
                    
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
                    var commandExecutor = new CommandExecutor(ServiceProvider
                        .GetRequiredService<IDbConnection>(), Substitute.For<ILogger<BaseSqlExecutor>>());

                    var queryExecutor = new QueryExecutor(ServiceProvider
                        .GetRequiredService<IDbConnection>(), Substitute.For<ILogger<BaseSqlExecutor>>());
                    
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


        [TestFixture]
        public class WithCommandEmbeddedInCommands
        {
            [Test]
            public void ShouldBeReceivable()
            {
                // arrange
                var idbConnection = Substitute.For<IDbConnection>();
                var commandExecutor = Substitute.For<CommandExecutor>(idbConnection, Substitute.For<ILogger<BaseSqlExecutor>>());
                var user = RandomValueGen.GetRandom<User>();

                var command = new InsertUser(user);
                var insertUserType = new InsertUserType(user.UserType);
                // act 
                commandExecutor.Execute(command);
                // assert
                Expect(commandExecutor).To.Have.Received(1).Execute(command);
                Expect(commandExecutor).To.Have.Received(1).Execute(insertUserType);
            }
        }
    }
}