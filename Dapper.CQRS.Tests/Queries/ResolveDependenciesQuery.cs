using System.Data;

namespace Dapper.CQRS.Tests.Queries
{
    public class ResolveDependenciesQuery : Query<IDbConnection>
    {
        public override void Execute()
        {
            Result = new SuccessResult<IDbConnection>(GetRequiredService<IDbConnection>());
        }
    }
}