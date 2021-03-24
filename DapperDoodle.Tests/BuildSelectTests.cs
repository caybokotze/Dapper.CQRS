using System;
using DapperDoodle.Tests.TestModels;
using NExpect;
using NUnit.Framework;
using static NExpect.Expectations;
using static PeanutButter.RandomGenerators.RandomValueGen;

namespace DapperDoodle.Tests
{
    [TestFixture]
    public class BuildSelectTests
    {
        [Test]
        public void ShouldReturnSnakeCaseSelectStatement()
        {
            // Arrange
            var expected = $"SELECT id, name, surname, email, date_created FROM `people`;";
            
            // Act
            var person = new Person
            {
                Id = GetRandomInt(),
                Name = Faker.Name.First(),
                Surname = Faker.Name.Last(),
                Email = Faker.Internet.Email(),
                DateCreated = DateTime.Now
            };
            
            var selectStatementBuilder = new TestBuildSelect<Person>(Case.SnakeCase);
            selectStatementBuilder.Execute();
            var actual = selectStatementBuilder.Result;
            
            // Assert
            Expect(actual).To.Equal(expected);
        }

        private class TestBuildSelect<T> : Query<string>
        {
            private Case Case { get;}

            public TestBuildSelect(Case @case)
            {
                Case = @case;
            }
            public override void Execute()
            {
                Result = this.BuildSelectStatement<T>(null, null, Case);
            }
        }
    }
}