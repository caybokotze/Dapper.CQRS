using System;
using System.Data;
using System.Transactions;

namespace Dapper.CQRS;

public class BaseConnection
{
    protected IDbConnection Connection => CreateOpenConnection();
    protected IServiceProvider Services => ConnectionConfiguration.ServiceProvider 
                                           ?? throw new InvalidOperationException("A registered service provider is not available. Register a service provider in the configuration builder to use internally");

    #region Internal use via configuration builder
    internal static ConnectionConfiguration Configuration => new();
    #endregion
    
    protected T GetRequiredService<T>() where T : notnull
    {
        return (T) Services.GetService(typeof(T)) ?? throw new InvalidOperationException($"No service of type {typeof(T).FullName} registered.");
    }

    protected T? GetService<T>() where T : notnull
    {
        return (T?) Services.GetService(typeof(T));
    }

    protected void SetConnectionName(string connectionName)
    {
        Configuration.ScopedConnectionName = connectionName;
    }
    
    protected void SetSplitOn(string splitOn)
    {
        Configuration.ScopedSplitOn = splitOn;
    }

    protected void ValidateTransaction()
    {
        if (Transaction.Current is null && ConnectionConfiguration.ValidateAmbientTransaction)
        {
            throw new InvalidOperationException("No ambient transaction found. Make sure you have configured ambient transactions or explicitly set ValidateTransaction to false.");
        }
    }

    /// <summary>
    /// Marked as static to make sure we don't open a connection twice if we're in a transaction
    /// if the Transaction.Current is null and we're not in a transaction, we open a new connection in the CreateOpenConnection method body
    /// </summary>
    private static IDbConnection? _dbConnection;
    
    internal IDbConnection CreateOpenConnection()
    {
        if (ConnectionConfiguration.ValidateAmbientTransaction)
        {
            ValidateTransaction();
        }
        
        if (!ConnectionConfiguration.ValidateAmbientTransaction)
        {
            if (Transaction.Current is not null && _dbConnection is not null)
            {
                if (_dbConnection.State != ConnectionState.Open)
                {
                    _dbConnection.Open();
                }
                
                return _dbConnection;
            }
        }

        if (ConnectionConfiguration.ConnectionFactory is null)
        {
            throw new InvalidOperationException("No connection factory registered. Register a connection factory in the configuration builder to use internally");
        }

        _dbConnection = Configuration.ScopedConnectionName is not null 
            ? ConnectionConfiguration.ConnectionFactory.Create(Configuration.ScopedConnectionName) 
            : ConnectionConfiguration.ConnectionFactory.Create();
            
        if (_dbConnection.State != ConnectionState.Open)
        {
            _dbConnection.Open();
        }

        return _dbConnection;
    }
}