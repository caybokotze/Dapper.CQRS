using System.Data;
using MySql.Data.MySqlClient;

namespace Dapper.CQRS.Tests.Utilities;

public class ConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public ConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public IDbConnection Create()
    {
        return new MySqlConnection(_connectionString);
    }

    public IDbConnection Create(string connectionName)
    {
        return new MySqlConnection(_connectionString);
    }
}