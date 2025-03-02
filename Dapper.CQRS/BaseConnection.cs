using System;
using System.Data;
using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace Dapper.CQRS;

public class BaseConnection
{
    protected IDbConnection Connection => CreateOpenConnection();
    protected IServiceProvider Services => ServiceProvider ?? throw new InvalidOperationException("A registered service provider is not available. Register a service provider in the configuration builder to use internally");

    #region Internal use via configuration builder

    internal static IDbConnectionFactory? ConnectionFactory { get; set; }
    internal static IServiceProvider? ServiceProvider { get; set; }
    internal static string DefaultSplitOn { get; set; } = "Id";
    
    internal static IsolationLevel DefaultIsolatedLevel = IsolationLevel.ReadCommitted;
    internal static int DefaultTimeout { get; set; } = 30;
    internal static bool ValidateAmbientTransaction { get; set; }
        
    #endregion
    
    internal string SplitOn { get; private set; } = DefaultSplitOn;
    private string? ConnectionName { get; set; }
    
    protected bool IsolateTransactions { get; set; }

    private IsolationLevel IsolationLevel { get; } = DefaultIsolatedLevel;
    
    protected ILogger<T> GetLogger<T>() where T : BaseConnection 
    {
        return Services.GetRequiredService<ILogger<T>>();
    }

    protected T GetRequiredService<T>() where T : notnull
    {
        return Services.GetRequiredService<T>();
    }

    protected void SetConnectionName(string connectionName)
    {
        ConnectionName = connectionName;
    }
    
    protected void SetSplitOn(string splitOn)
    {
        SplitOn = splitOn;
    }

    protected TransactionScope CreateTransactionScope()
    {
        if (Transaction.Current is not null)
        {
            return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = Transaction.Current.IsolationLevel,
                Timeout = TimeSpan.FromSeconds(DefaultTimeout)
            });
        }
        
        return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
        {
            IsolationLevel = IsolationLevel,
            Timeout = TimeSpan.FromSeconds(DefaultTimeout)
        });
    }

    private IDbConnection? _dbConnection;
    
    internal IDbConnection CreateOpenConnection()
    {
        if (!IsolateTransactions)
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

        if (ConnectionFactory is null)
        {
            throw new InvalidOperationException("No connection factory registered. Register a connection factory in the configuration builder to use internally");
        }

        _dbConnection = ConnectionName is not null 
            ? ConnectionFactory.Create(ConnectionName) 
            : ConnectionFactory.Create();
            
        if (_dbConnection.State != ConnectionState.Open)
        {
            _dbConnection.Open();
        }

        return _dbConnection;
    }
}