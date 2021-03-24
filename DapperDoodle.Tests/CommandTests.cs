using System;
using System.Transactions;
using DapperDoodle.Tests.TestModels;
using NExpect;
using NUnit.Framework;
using static NExpect.Expectations;

namespace DapperDoodle.Tests
{
    [TestFixture]
    public class CommandTests : ResolveExecutors
    {
        [TestFixture]
        public class Behaviour
        {
            [Test]
            public void ShouldBuildInsertForMySql()
            {
                // Arrange
                var command = Create();
                command.Dbms = DBMS.MySQL;

                // Act
                var insertStatement = command.BuildInsertStatement<Person>(null, Case.Lowercase);
                var expected = $"INSERT INTO `people` (id, name, surname, email, datecreated) " +
                               $"VALUES (@Id, @Name, @Surname, @Email, @DateCreated); " +
                               $"SELECT LAST_INSERT_ID();";
                
                // Assert
                Expect(insertStatement).To.Equal(expected);
            }

            [Test]
            public void ShouldBuildInsertForSqlLite()
            {
                // Arrange
                var command = Create();
                command.Dbms = DBMS.SQLite;

                // Act
                var insertStatement = command.BuildInsertStatement<Person>(null, Case.SnakeCase);
                var expected = $"INSERT INTO people (id, name, surname, email, date_created) " +
                               $"VALUES (@Id, @Name, @Surname, @Email, @DateCreated); " +
                               $"SELECT last_insert_rowid();";
                
                // Assert
                Expect(insertStatement).To.Equal(expected);
            }
            
            [Test]
            public void ShouldBuildInsertForMsSql()
            {
                // Arrange
                var command = Create();
                command.Dbms = DBMS.MSSQL;

                // Act
                var insertStatement = command.BuildInsertStatement<Person>(null, Case.SnakeCase);
                var expected = $"INSERT INTO [people] (id, name, surname, email, date_created) " +
                               $"VALUES (@Id, @Name, @Surname, @Email, @DateCreated); " +
                               $"SELECT SCOPE_IDENTITY();";
                
                // Assert
                Expect(insertStatement).To.Equal(expected);
            }
        }

        [TestFixture]
        public class Transactions
        {
            [Test]
            public void ShouldInsertRecordForMySql()
            {
                using (var scope = new TransactionScope())
                {
                    // todo: complete...
                }
            }
            
            [Test]
            public void ShouldUpdateRecordForMySql()
            {
                using (var scope = new TransactionScope())
                {
                    // todo: complete...
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
                BuildInsert<Person>(_person);
            }
        }

        private class RandomCommand : Command
        {
            public override void Execute()
            {
                
            }
        }

        private static Command Create()
        {
            return new RandomCommand();
        }
    }
}