using System.Data;

namespace Dapper.CQRS.Tests.TestQueries;

public class QueryDependency<T> : Query<T> where T : notnull
{
    public override T Execute()
    {
        return GetRequiredService<T>();
    }
}