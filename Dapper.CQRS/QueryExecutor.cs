using System;
using System.Data;

namespace Dapper.CQRS
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IDbConnection _connection;

        public QueryExecutor(IDbConnection connection)
        {
            _connection = connection;
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
            query.InitialiseIDbConnection(_connection);
            ExecuteWithNoResult(query);
            return query.Result;
        }
    }
}