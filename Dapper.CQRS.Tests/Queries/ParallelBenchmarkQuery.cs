using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute.Routing.Handlers;
using NUnit.Framework;

namespace Dapper.CQRS.Tests.Queries
{
    public class ParallelBenchmarkQuery : Query<int>
    {
        public override async Task<int> Execute()
        {
            const string sql = "select benchmark(10000000, md5('when will it end?'));";
            var taskList = new List<Task<int>>
            {
                Task.Run(() => QueryFirst<int>(sql)),
                Task.Run(() => QueryFirst<int>(sql)),
                Task.Run(() => QueryFirst<int>(sql))
            };

            // await Task.WhenAll(taskList);
            //
            // foreach (var item in taskList)
            // {
            //     await Task.Run(() => item);
            // }

            // Parallel.ForEach(taskList, task =>
            // {
            //     task.Start();
            // });

            await Task.WhenAll(taskList);

            return 0;
        }
    }
}