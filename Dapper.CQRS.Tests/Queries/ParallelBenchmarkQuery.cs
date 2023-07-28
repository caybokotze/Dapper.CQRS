using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper.CQRS.Tests.Queries
{
    public class ParallelBenchmarkQuery : QueryAsync<int>
    {
        public override async Task ExecuteAsync()
        {
            const string sql = "select benchmark(10000000, md5('when will it end?'));";
            
            var taskList = new List<Task<int>>
            {
                Task.Run(() => QueryFirstAsync<int>(sql)),
                Task.Run(() => QueryFirstAsync<int>(sql)),
                Task.Run(() => QueryFirstAsync<int>(sql))
            };
            
            var result = await Task.WhenAll(taskList);

            Result = new SuccessResult<int>(1);
        }
    }
}