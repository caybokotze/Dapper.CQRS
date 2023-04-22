using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.CQRS.Tests.Queries
{
    public class ResolveDependenciesQuery : Query<IDbConnection>
    {
        public override Task<IDbConnection> Execute()
        {
            return Task.FromResult(Sp.GetRequiredService<IDbConnection>());
        }
    }
}