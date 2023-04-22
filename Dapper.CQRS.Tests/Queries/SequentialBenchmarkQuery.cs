using System.Threading.Tasks;

namespace Dapper.CQRS.Tests.Queries
{
    public class SequentialBenchmarkQuery : Query<int>
    {
        public override async Task<int> Execute()
        {
            const string sql = "select benchmark(10000000, md5('when will it end?'));";
            await QueryFirst<int>(sql);
            await QueryFirst<int>(sql);
            return await QueryFirst<int>(sql);
        }
    }
}