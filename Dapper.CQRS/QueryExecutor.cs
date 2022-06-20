using System;
using Microsoft.Extensions.Logging;

namespace Dapper.CQRS
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IExecutable _executable;
        private readonly IQueryable _queryable;
        private readonly ILoggerFactory _loggerFactory;

        // ReSharper disable once ContextualLoggerProblem
        public QueryExecutor(IExecutable executable, IQueryable queryable, ILoggerFactory loggerFactory)
        {
            _executable = executable;
            _queryable = queryable;
            _loggerFactory = loggerFactory;
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
            query.Initialise(_executable, _queryable, _loggerFactory);
            ExecuteWithNoResult(query);
            return query.Result;
        }
    }
}