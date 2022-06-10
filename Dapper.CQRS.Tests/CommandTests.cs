using System.Collections.Generic;
using System.Data;
using System.Transactions;
using Dapper.CQRS.Tests.Commands;
using Dapper.CQRS.Tests.TestModels;
using Dapper.CQRS.Tests.Utilities;
using GenericSqlBuilder;
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
                    var queryable = ServiceProvider.GetRequiredService<IQueryable>();
                    var executor = ServiceProvider.GetRequiredService<IExecutable>();
                    // arrange
                    var commandExecutor = new CommandExecutor(executor, queryable, Substitute.For<ILogger<BaseSqlExecutor>>());

                    var queryExecutor = new QueryExecutor(executor, queryable, Substitute.For<ILogger<BaseSqlExecutor>>());
                    
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
                    var queryable = ServiceProvider.GetRequiredService<IQueryable>();
                    var executor = ServiceProvider.GetRequiredService<IExecutable>();
                    
                    var commandExecutor = new CommandExecutor(executor, queryable, Substitute.For<ILogger<BaseSqlExecutor>>());

                    var queryExecutor = new QueryExecutor(executor, queryable, Substitute.For<ILogger<BaseSqlExecutor>>());
                    
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
            public void ShouldBeMockable()
            {
                // arrange
                var queryable = Substitute.For<IQueryable>();
                var executable = Substitute.For<IExecutable>();
                
                var commandExecutor =
                    Substitute.For<CommandExecutor>(executable, queryable, Substitute.For<ILogger<BaseSqlExecutor>>());
                var user = GetRandom<User>();
                var sut = new InsertUser(user);
                
                var sql = new SqlBuilder()
                    .Insert<User>("users", i =>
                    {
                        i.RemoveProperty(nameof(User.UserType));
                        i.RemoveProperty(nameof(User.UserDetails));
                    })
                    .Values()
                    .Build();

                var insertUserTypeQuery = new SqlBuilder()
                    .Insert<UserType>("user_types")
                    .Values()
                    .AppendStatement()
                    .Select()
                    .LastInserted(Version.MySql);
                
                var randomNumber = GetRandomInt();
                queryable.QueryFirst<int>(insertUserTypeQuery).Returns(randomNumber);

                // act
                var result = commandExecutor.Execute(sut);
                // assert
                Expect(executable).To.Have.Received(1).Execute(sql, user);
                Expect(queryable).To.Have.Received(1).QueryFirst<int>(insertUserTypeQuery);
                Expect(result).To.Equal(randomNumber);
            }
        }
    }
}