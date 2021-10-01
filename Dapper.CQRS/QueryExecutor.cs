using System;
using Dapper.CQRS.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.CQRS
{
    public class QueryExecutor : IQueryExecutor
    {
        public CQRSSqlExecutorOptions Options { get; }

        public QueryExecutor(IServiceProvider services)
        {
            Options = services.GetService<CQRSSqlExecutorOptions>();
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
            query.InitialiseDependencies(Options);
            ExecuteWithNoResult(query);
            return query.Result;
        }
    }
}