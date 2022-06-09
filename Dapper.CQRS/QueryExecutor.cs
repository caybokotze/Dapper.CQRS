using System;
using System.Data;
using Microsoft.Extensions.Logging;

namespace Dapper.CQRS
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IExecutor _executor;
        private readonly IQueryable _queryable;
        private readonly ILogger<BaseSqlExecutor> _logger;

        public QueryExecutor(IExecutor executor, IQueryable queryable, ILogger<BaseSqlExecutor> logger)
        {
            _executor = executor;
            _queryable = queryable;
            _logger = logger;
        }

        private void ExecuteWithNoResult(Query query)
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            query.Execute();
        }

        public T Execute<T>(Query<T> query)
        {
            query.Initialise(_executor, _queryable, _logger);
            ExecuteWithNoResult(query);
            return query.Result;
        }
    }
}