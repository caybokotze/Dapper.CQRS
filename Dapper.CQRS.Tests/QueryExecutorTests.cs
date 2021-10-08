using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using static PeanutButter.RandomGenerators.RandomValueGen;

namespace Dapper.CQRS.Tests
{
    [TestFixture]
    public class QueryExecutorTests
    {
        [TestFixture]
        public class Registrations : TestBase
        {
            [Test]
            public void AssertThatIApplicationBuilderRegistersIQueryExecutor()
            {
                var queryExecutor = ServiceProvider
                    .GetService<IQueryExecutor>();

                var actual = GetRandomInt();
                
                var expected = queryExecutor?
                    .Execute(new QueryInheritor(actual));
            
                Assert.That(expected.Equals(actual));
            }
        }

        private class QueryInheritor : Query<int>
        {
            private readonly int _expectedReturnValue;

            public QueryInheritor(int expectedReturnValue)
            {
                _expectedReturnValue = expectedReturnValue;
            }
            
            public override void Execute()
            {
                Result = QueryFirst<int>($"SELECT {_expectedReturnValue};");
            }
        }
    }
}