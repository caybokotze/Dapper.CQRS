using System;
using System.Collections.Generic;
using DapperDoodle.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DapperDoodle
{
    public class QueryExecutor : IQueryExecutor
    {
        private IBaseSqlExecutorOptions Options { get; }

        public QueryExecutor(IServiceProvider services)
        {
            Options = services.GetService<IBaseSqlExecutorOptions>();
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