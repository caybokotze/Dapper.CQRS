﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dapper.CQRS;

/// <summary>
/// A asynchronous SqlExecutor which exposes the ServiceProvider (to resolve dependencies) and Dapper methods.
/// All dapper methods are marked as virtual to allow for mocked return values with Moq or NSubstitute.
/// </summary>
public class SqlExecutorAsync
{
    protected SqlExecutorAsync()
    {
        _dbConnectionSemaphore = new SemaphoreSlim(1,1);
    }
    
    private ILogger? _logger;
    private IServiceProvider? _serviceProvider;

    protected ILogger Logger => _logger
                                ?? throw new InvalidOperationException(
                                    "The logger has not been correctly initialised. Make sure this is being executed via a ICommandExecutor / IQueryExecutor");

    protected IDbConnection Connection => CreateOpenConnection();

    protected T GetRequiredService<T>() where T : notnull
    {
        if (_serviceProvider is null)
        {
            throw new InvalidOperationException("The IServiceProvider instance is null. Check to see whether this has properly been initialised. The command/query needs to be executed via a IQueryExecutor or ICommandExecutor .Execute method");
        }

        return _serviceProvider.GetRequiredService<T>();
    }

    internal void InitialiseExecutor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = serviceProvider.GetRequiredService<ILogger<SqlExecutor>>();
    }

    private readonly SemaphoreSlim _dbConnectionSemaphore;
    private IDbConnection? _dbConnection = null;
    
    private IDbConnection CreateOpenConnection()
    {
        _dbConnectionSemaphore.Wait();
        
        try
        {
            if (_serviceProvider is null)
            {
                throw new InvalidOperationException("The IDbConnection instance is null. Check to see whether this has properly been initialised. The command/query needs to be executed via a IQueryExecutor or ICommandExecutor .Execute method");
            }
            
            // Prevent the transaction from being disposed if there is a active transaction.
            if (Transaction.Current is not null && _dbConnection is not null)
            {
                _dbConnection.Open();
                return _dbConnection;
            }
                
            _dbConnection =  _serviceProvider.GetRequiredService<IDbConnection>();

            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            return _dbConnection;
        }
        finally
        {
            _dbConnectionSemaphore.Release();
        }
    }

    public virtual async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null)
    {
        using var connection = CreateOpenConnection();
        return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
    }
        
    public virtual async Task<T> QueryFirstAsync<T>(string sql, object? parameters = null)
    {
        using var connection = CreateOpenConnection();
        return await connection.QueryFirstAsync<T>(sql, parameters);
    }

    public virtual async Task<IEnumerable<T>> QueryListAsync<T>(string sql, object? parameters = null)
    {
        using var connection = CreateOpenConnection();
        return await connection.QueryAsync<T>(sql, parameters);
    }

    public virtual async Task<IEnumerable<TOut>> QueryListAsync<TIn, T2, TOut>(string sql,
        Func<TIn, T2, TOut> map,
        object? parameters = null)
    {
        using var connection = CreateOpenConnection();
        return await connection.QueryAsync(sql, map, parameters);
    }

    public virtual async Task<IEnumerable<TOut>> QueryListAsync<TIn, T2, T3, TOut>(string sql,
        Func<TIn, T2, T3, TOut> map,
        object? parameters = null)
    {
        using var connection = CreateOpenConnection();
        return await connection.QueryAsync(sql, map, parameters);
    }

    public virtual async Task<IEnumerable<TOut>> QueryListAsync<TIn, T2, T3, T4, TOut>(string sql,
        Func<TIn, T2, T3, T4, TOut> map,
        object? parameters = null)
    {
        using var connection = CreateOpenConnection();
        return await connection.QueryAsync(sql, map, parameters);
    }

    public virtual async Task<IEnumerable<TOut>> QueryListAsync<TIn, T2, T3, T4, T5, TOut>(string sql,
        Func<TIn, T2, T3, T4, T5, TOut> map,
        object? parameters = null)
    {
        using var connection = CreateOpenConnection();
        return await connection.QueryAsync(sql, map, parameters);
    }

    public virtual async Task<IEnumerable<TOut>> QueryListAsync<TIn, T2, T3, T4, T5, T6, TOut>(string sql,
        Func<TIn, T2, T3, T4, T5, T6, TOut> map,
        object? parameters = null)
    {
        using var connection = CreateOpenConnection();
        return await connection.QueryAsync(sql, map, parameters);
    }

    public virtual async Task<IEnumerable<TOut>> QueryListAsync<TIn, T2, T3, T4, T5, T6, T7, TOut>(string sql,
        Func<TIn, T2, T3, T4, T5, T6, T7, TOut> map,
        object? parameters = null)
    {
        using var connection = CreateOpenConnection();
        return await connection.QueryAsync(sql, map, parameters);
    }

    public virtual async Task<int> ExecuteAsync(string sql, object? parameters = null)
    {
        if (string.IsNullOrWhiteSpace(sql))
        {
            throw new ArgumentException("Please specify a value for the sql attribute.");
        }
        using var connection = CreateOpenConnection();
        return await connection.ExecuteAsync(sql, parameters);
    }
}