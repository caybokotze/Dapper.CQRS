using System.Data;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.DependencyInjection;
using NExpect;
using NUnit.Framework;

namespace Dapper.CQRS.Tests
{
    [TestFixture]
    public class QueryTests
    {
        public class User
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
        
        [TestFixture]
        public class SelectionTests : TestBase
        {
            [Test]
            public void ShouldSelectUser()
            {
                // arrange
                var queryExecutor = new QueryExecutor(ServiceProvider.GetRequiredService<IDbConnection>());
                // act
                var user = queryExecutor
                    .Execute(new GenericQuery<User>("SELECT * FROM users WHERE id = @Id;",
                    new {
                        Id = 41
                    }));
                // assert
                Expectations.Expect(user.Id).To.Equal(41);
            }
        }
    }
}