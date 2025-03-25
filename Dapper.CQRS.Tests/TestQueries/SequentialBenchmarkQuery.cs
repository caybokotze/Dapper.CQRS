using System.Threading.Tasks;

namespace Dapper.CQRS.Tests.TestQueries;

public class SequentialBenchmarkQuery : QueryAsync<int>
{
    public override async Task<int> ExecuteAsync()
    {
        const string sql = "select benchmark(10000000, md5('when will it end?'));";
        await QueryFirstAsync<int>(sql);
        await QueryFirstAsync<int>(sql);
        await QueryFirstAsync<int>(sql);

        return 1;
    }
}