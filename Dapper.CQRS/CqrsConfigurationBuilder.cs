using System;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace Dapper.CQRS;

public class CqrsConfigurationBuilder
{
    /// <summary>
    /// A helper to automatically register the required dependencies in the service container.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <returns></returns>
    public CqrsConfigurationBuilder WithRequiredServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IQueryExecutor, QueryExecutor>();
        serviceCollection.AddTransient<ICommandExecutor, CommandExecutor>();
        return this;
    }

    /// <summary>
    /// Provides the service provider instance to resolve internal dependencies via a query/command
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public CqrsConfigurationBuilder WithServiceProvider(IServiceProvider serviceProvider)
    {
        BaseConnection.ServiceProvider = serviceProvider;
        return this;
    }
    
    /// <summary>
    /// Specify the IDbConnection instance to use. This will create an internal factory. However, using different IDbConnection instances is not supported with this method.
    /// </summary>
    /// <param name="connectionFactory"></param>
    /// <returns></returns>
    public CqrsConfigurationBuilder WithConnectionFactory(Func<IDbConnection> connectionFactory)
    {
        BaseConnection.ConnectionFactory = new InternalDbConnectionFactory(connectionFactory());
        return this;
    }
    
    /// <summary>
    /// Provide a custom connection factory instance.
    /// </summary>
    /// <param name="connectionFactory"></param>
    /// <returns></returns>
    public CqrsConfigurationBuilder WithConnectionFactory(IDbConnectionFactory connectionFactory)
    {
        BaseConnection.ConnectionFactory = connectionFactory;
        return this;
    }
    
    /// <summary>
    /// The integer value is specified in seconds. This is set to 30 seconds by default.
    /// </summary>
    /// <param name="timeout"></param>
    /// <returns></returns>
    public CqrsConfigurationBuilder WithDefaultQueryTimeout(int timeout)
    {
        BaseConnection.DefaultTimeout = timeout;
        return this;
    }

    public CqrsConfigurationBuilder WithAmbientTransactionRequired()
    {
        BaseConnection.ValidateAmbientTransaction = true;
        return this;
    }
    
    public CqrsConfigurationBuilder WithAmbientTransactionOptional()
    {
        BaseConnection.ValidateAmbientTransaction = false;
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
        BaseConnection.ConnectionFactory = null;
        BaseConnection.ServiceProvider = null;
        BaseConnection.DefaultTimeout = 30;
        return this;
    }

    /// <summary>
    /// Specifies the default isolation level to use when creating new transaction scopes.
    /// </summary>
    /// <param name="isolationLevel"></param>
    /// <returns></returns>
    public CqrsConfigurationBuilder WithDefaultIsolationLevel(IsolationLevel isolationLevel)
    {
        BaseConnection.DefaultIsolatedLevel = isolationLevel;
        return this;
    }

    /// <summary>
    /// Define the default splitOn value which is set to 'Id' by default.
    /// </summary>
    /// <param name="splitOn"></param>
    /// <returns></returns>
    public CqrsConfigurationBuilder WithDefaultSplitOn(string splitOn)
    {
        BaseConnection.DefaultSplitOn = splitOn;
        return this;
    }
    
}