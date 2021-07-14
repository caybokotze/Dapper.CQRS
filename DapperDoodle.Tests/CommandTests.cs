using System;
using System.Transactions;
using Dapper.CQRS;
using DapperDoodle.Tests.TestModels;
using NUnit.Framework;

namespace DapperDoodle.Tests
{
    [TestFixture]
    public class CommandTests
    {
        private IServiceProvider _serviceProvider;
        private ICommandExecutor _commandExecutor;

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