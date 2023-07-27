using System;
using System.Threading.Tasks;

namespace Dapper.CQRS
{
    public interface IQueryExecutor
    {
        Result<T> Execute<T>(Query<T> query);
        Task<Result<T>> ExecuteAsync<T>(QueryAsync<T> query);
    }
    
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Result<T> Execute<T>(Query<T> query)
        {
            ExecuteWithNoResult(query);
            return query.Result;
        }

        public async Task<Result<T>> ExecuteAsync<T>(QueryAsync<T> query)
        {
            await ExecuteWithNoResultAsync(query);
            return query.Result;
        }

        private void ExecuteWithNoResult(Query query)
        {
            query.QueryExecutor = this;
            query.InitialiseExecutor(_serviceProvider);
            query.Execute();
        }

        private async Task ExecuteWithNoResultAsync(QueryAsync query)
        {
            query.QueryExecutor = this;
            query.InitialiseExecutor(_serviceProvider);
            await query.ExecuteAsync();
        }
    }
}