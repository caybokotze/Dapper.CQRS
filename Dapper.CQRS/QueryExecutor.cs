using System;
using System.Threading.Tasks;

namespace Dapper.CQRS
{
    public interface IQueryExecutor
    {
        Task<T> Execute<T>(Query<T> query);
    }
    
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<T> Execute<T>(Query<T> query)
        {
            query.Initialise(_serviceProvider);
            return await query.Execute();
        }
    }
}