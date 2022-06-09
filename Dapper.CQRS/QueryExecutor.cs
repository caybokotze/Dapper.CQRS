using System;
using System.Data;
using Microsoft.Extensions.Logging;

namespace Dapper.CQRS
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<BaseSqlExecutor> _logger;

        public QueryExecutor(IDbConnection connection, ILogger<BaseSqlExecutor> logger)
        {
            _connection = connection;
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
            query.Initialise(_connection, _logger);
            ExecuteWithNoResult(query);
            return query.Result;
        }
    }
}