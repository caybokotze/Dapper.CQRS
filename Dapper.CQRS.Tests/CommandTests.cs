using System;
using System.Data;
using System.Transactions;
using Dapper.CQRS.Tests.TestModels;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using NExpect;
using NUnit.Framework;
using Org.BouncyCastle.Crypto.Tls;
using static NExpect.Expectations;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace Dapper.CQRS.Tests
{
    [TestFixture]
    public class CommandTests
    {
        public class User
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
        
        [TestFixture]
        public class Transactions : TestBase
        {
            [Test]
            public void ShouldInsertRecordForMySql()
            {
                using (new TransactionScope())
                {
                    // arrange
                    var commandExecutor = new CommandExecutor(ServiceProvider
                        .GetRequiredService<IDbConnection>());

                    var queryExecutor = new QueryExecutor(ServiceProvider
                        .GetRequiredService<IDbConnection>());
                    
                    var user = new User
                    {
                        FirstName = Faker.Name.First(),
                        LastName = Faker.Name.Last()
                    };
                    
                    // act
                    var userId = commandExecutor
                        .Execute(new GenericCommand<int>(
                            "INSERT INTO users (first_name, last_name) VALUES (@FirstName, @LastName); SELECT LAST_INSERT_ID();",
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
            public void ShouldUpdateRecordForMySql()
            {
                using (new TransactionScope())
                {
                    // arrange
                    var commandExecutor = new CommandExecutor(ServiceProvider
                        .GetRequiredService<IDbConnection>());

                    var queryExecutor = new QueryExecutor(ServiceProvider
                        .GetRequiredService<IDbConnection>());
                    
                    var user = new User
                    {
                        FirstName = Faker.Name.First(),
                        LastName = Faker.Name.Last()
                    };
                    
                    // act
                    var userId = commandExecutor
                        .Execute(new GenericCommand<int>(
                            "INSERT INTO users (first_name, last_name) VALUES (@FirstName, @LastName); SELECT LAST_INSERT_ID();",
                            user));

                    user.Id = userId;
                    
                    var expectedFirstUser =
                        queryExecutor.Execute(new GenericQuery<User>("SELECT * FROM users where id = @Id;",
                            new { Id = userId }));

                    var updatedUser = new User
                    {
                        Id = userId,
                        FirstName = Faker.Name.First(),
                        LastName = Faker.Name.Last()
                    };

                    commandExecutor
                        .Execute(new GenericCommand<int>("UPDATE users SET first_name = @FirstName, last_name = @LastName WHERE id = @Id; SELECT LAST_INSERT_ID();", updatedUser));

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
            public void ShouldDeleteRecordForMySql()
            {
                using (var scope = new TransactionScope())
                {
                    // todo: complete...
                }
            }
        }

        public class InsertPerson : Command
        {
            private readonly Person _person;

            public InsertPerson(Person person)
            {
                _person = person;
            }
            
            public override void Execute()
            {
                // BuildInsert<Person>(_person);
            }
        }

        public class RandomCommand : Command
        {
            public override void Execute()
            {
                
            }
        }

        public static Command Create()
        {
            return new RandomCommand();
        }
    }
}