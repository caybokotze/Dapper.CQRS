using System;
using System.Data;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace Dapper.CQRS;

public class CqrsConfigurationBuilder
{
    /// <summary>
    /// Provides the service provider instance to resolve internal dependencies via a query/command
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public CqrsConfigurationBuilder WithServiceProvider(IServiceProvider serviceProvider)
    {
        ConnectionConfiguration.ServiceProvider = serviceProvider;
        return this;
    }

    /// <summary>
    /// If there is no ambient transaction set, create a new transaction scope with the specified scopeOption and isolationLevel.
    /// </summary>
    /// <param name="scopeOption"></param>
    /// <param name="isolationLevel"></param>
    /// <returns></returns>
    public CqrsConfigurationBuilder WithDefaultTransactionScope(TransactionScopeOption scopeOption, IsolationLevel isolationLevel)
    {
        ConnectionConfiguration.CreateTransaction = true;
        ConnectionConfiguration.DefaultTransactionScopeOption = scopeOption;
        ConnectionConfiguration.DefaultIsolationLevel = isolationLevel;
        return this;
    }

    public CqrsConfigurationBuilder WithAutoTransactionRollback()
    {
        if (!ConnectionConfiguration.CreateTransaction)
        {
            throw new InvalidOperationException("A transaction is required for this to be configured. Please use WithDefaultTransactionScope first");
        }

        ConnectionConfiguration.AutoRollback = true;
        return this;
    }
        
    /// <summary>
    /// Specify the IDbConnection instance to use. This will create an internal factory. However, using different IDbConnection instances is not supported with this method.
    /// </summary>
    /// <param name="connectionFactory"></param>
    /// <returns></returns>
    public CqrsConfigurationBuilder WithConnectionFactory(Func<IDbConnection> connectionFactory)
    {
        ConnectionConfiguration.ConnectionFactory = new InternalDbConnectionFactory(connectionFactory());
        return this;
    }
    
    /// <summary>
    /// Provide a custom connection factory instance.
    /// </summary>
    /// <param name="connectionFactory"></param>
    /// <returns></returns>
    public CqrsConfigurationBuilder WithConnectionFactory(IDbConnectionFactory connectionFactory)
    {
        ConnectionConfiguration.ConnectionFactory = connectionFactory;
        return this;
    }
    
    /// <summary>
    /// The integer value is specified in seconds. This is set to 30 seconds by default.
    /// </summary>
    /// <param name="timeout"></param>
    /// <returns></returns>
    public CqrsConfigurationBuilder WithDefaultQueryTimeout(int timeout)
    {
        ConnectionConfiguration.DefaultTimeout = timeout;
        return this;
    }

    public CqrsConfigurationBuilder WithAmbientTransactionRequired()
    {
        ConnectionConfiguration.ValidateAmbientTransaction = true;
        return this;
    }
    
    public CqrsConfigurationBuilder WithAmbientTransactionOptional()
    {
        ConnectionConfiguration.ValidateAmbientTransaction = false;
        return this;
    }

    /// <summary>
    /// Enables the dapper setting for snake case property mappings
    /// </summary>
    /// <returns></returns>
    public CqrsConfigurationBuilder WithSnakeCaseMappingsEnabled()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        return this;
    }

    /// <summary>
    /// Disables the dapper setting for snake case property mappings
    /// </summary>
    /// <returns></returns>
    public CqrsConfigurationBuilder WithSnakeCaseMappingsDisabled()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = false;
        return this;
    }

    /// <summary>
    /// Resets the connection factory and service provider.
    /// </summary>
    /// <returns></returns>
    public CqrsConfigurationBuilder Reset()
    {
        ConnectionConfiguration.ConnectionFactory = null;
        ConnectionConfiguration.ServiceProvider = null;
        ConnectionConfiguration.DefaultTimeout = 30;
        ConnectionConfiguration.CreateTransaction = false;
        ConnectionConfiguration.ValidateAmbientTransaction = false;
        ConnectionConfiguration.AutoRollback = false;
        return this;
    }
    
    /// <summary>
    /// Define the default splitOn value which is set to 'Id' by default.
    /// </summary>
    /// <param name="splitOn"></param>
    /// <returns></returns>
    public CqrsConfigurationBuilder WithDefaultSplitOn(string splitOn)
    {
        ConnectionConfiguration.DefaultSplitOn = splitOn;
        return this;
    }
    
}