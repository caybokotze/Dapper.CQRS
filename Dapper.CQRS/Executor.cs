using System.Data;

namespace Dapper.CQRS
{
    public class Executor : IExecutor
    {
        private readonly IDbConnection _connection;

        public Executor(IDbConnection connection)
        {
            _connection = connection;
        }

        public int Execute(string sql, object parameters = null)
        {
            return _connection.Execute(sql, parameters);
        }
    }
}