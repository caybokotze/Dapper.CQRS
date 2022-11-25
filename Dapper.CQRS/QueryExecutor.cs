using System;
using System.Data;
using Microsoft.Extensions.Logging;

namespace Dapper.CQRS
{
    public interface IQueryExecutor
    {
        T Execute<T>(Query<T> query);
    }
    
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T Execute<T>(Query<T> query)
        {
            query.Initialise(_serviceProvider);
            return query.Execute();
        }
    }
}