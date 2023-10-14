using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper.CQRS.Tests.Queries;

public class ParallelBenchmarkQuery : QueryAsync<bool>
{
    public override async Task<bool> ExecuteAsync()
    {
        const string sql = "select benchmark(10000000, md5('when will it end?'));";
            
        var taskList = new List<Task<int>>
        {
            Task.Run(() => QueryFirstAsync<int>(sql)),
            Task.Run(() => QueryFirstAsync<int>(sql)),
            Task.Run(() => QueryFirstAsync<int>(sql))
        };
            
        var result = await Task.WhenAll(taskList);

        return true; 
    }
}