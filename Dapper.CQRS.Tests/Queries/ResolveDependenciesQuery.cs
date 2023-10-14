using System.Data;

namespace Dapper.CQRS.Tests.Queries
{
    public class ResolveDependenciesQuery : Query<IDbConnection>
    {
        public override IDbConnection Execute()
        {
            return GetRequiredService<IDbConnection>();
        }
    }
}