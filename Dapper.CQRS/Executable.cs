using System.Data;

namespace Dapper.CQRS
{
    public class Executable : IExecutable
    {
        private readonly IDbConnection _connection;

        public Executable(IDbConnection connection)
        {
            _connection = connection;
        }

        public int Execute(string sql, object parameters = null)
        {
            return _connection.Execute(sql, parameters);
        }
    }
}