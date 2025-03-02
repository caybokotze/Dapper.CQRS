using System;
using System.Data;

namespace Dapper.CQRS;

internal class InternalDbConnectionFactory : IDbConnectionFactory
{
    private readonly IDbConnection _dbConnection;

    public InternalDbConnectionFactory(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    
    public IDbConnection Create()
    {
        return _dbConnection;
    }

    public IDbConnection Create(string connectionName)
    {
        throw new InvalidOperationException("This needs to be configured by implementing a custom IDbConnectionFactory instance. Do not use the Func`IDbConnection method on the configuration builder. Instead provide your own factory in the configuration");
    }
}