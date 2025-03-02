using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper.CQRS;

/// <summary>
/// A asynchronous SqlExecutor which exposes the ServiceProvider (to resolve dependencies) and Dapper methods.
/// All dapper methods are marked as virtual to allow for mocked return values with Moq or NSubstitute.
/// </summary>
public class SqlExecutorAsync : BaseConnection
{
    public virtual async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null)
    {
        using var scope = CreateTransactionScope();
        using var connection = CreateOpenConnection();
        return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters, commandTimeout: DefaultTimeout);
    }
        
    public virtual async Task<T> QueryFirstAsync<T>(string sql, object? parameters = null)
    {
        using var connection = CreateOpenConnection();
        return await connection.QueryFirstAsync<T>(sql, parameters, commandTimeout: DefaultTimeout);
    }

    public virtual async Task<IEnumerable<T>> QueryListAsync<T>(string sql, object? parameters = null)
    {
        using var connection = CreateOpenConnection();
        return await connection.QueryAsync<T>(sql, parameters, commandTimeout: DefaultTimeout);
    }

    public virtual async Task<IEnumerable<TOut>> QueryListAsync<TIn, T2, TOut>(string sql,
        Func<TIn, T2, TOut> map,
        object? parameters = null)
    {
        using var connection = CreateOpenConnection();
        return await connection.QueryAsync(sql, map, parameters, commandTimeout: DefaultTimeout, splitOn: SplitOn);
    }

    public virtual async Task<IEnumerable<TOut>> QueryListAsync<TIn, T2, T3, TOut>(string sql,
        Func<TIn, T2, T3, TOut> map,
        object? parameters = null)
    {
        using var connection = CreateOpenConnection();
        return await connection.QueryAsync(sql, map, parameters, commandTimeout: DefaultTimeout, splitOn: SplitOn);
    }

    public virtual async Task<IEnumerable<TOut>> QueryListAsync<TIn, T2, T3, T4, TOut>(string sql,
        Func<TIn, T2, T3, T4, TOut> map,
        object? parameters = null)
    {
        using var connection = CreateOpenConnection();
        return await connection.QueryAsync(sql, map, parameters, commandTimeout: DefaultTimeout, splitOn: SplitOn);
    }

    public virtual async Task<IEnumerable<TOut>> QueryListAsync<TIn, T2, T3, T4, T5, TOut>(string sql,
        Func<TIn, T2, T3, T4, T5, TOut> map,
        object? parameters = null)
    {
        using var connection = CreateOpenConnection();
        return await connection.QueryAsync(sql, map, parameters, commandTimeout: DefaultTimeout, splitOn: SplitOn);
    }

    public virtual async Task<IEnumerable<TOut>> QueryListAsync<TIn, T2, T3, T4, T5, T6, TOut>(string sql,
        Func<TIn, T2, T3, T4, T5, T6, TOut> map,
        object? parameters = null)
    {
        using var connection = CreateOpenConnection();
        return await connection.QueryAsync(sql, map, parameters, commandTimeout: DefaultTimeout, splitOn: SplitOn);
    }

    public virtual async Task<IEnumerable<TOut>> QueryListAsync<TIn, T2, T3, T4, T5, T6, T7, TOut>(string sql,
        Func<TIn, T2, T3, T4, T5, T6, T7, TOut> map,
        object? parameters = null)
    {
        using var connection = CreateOpenConnection();
        return await connection.QueryAsync(sql, map, parameters, commandTimeout: DefaultTimeout, splitOn: SplitOn);
    }

    public virtual async Task<int> ExecuteAsync(string sql, object? parameters = null)
    {
        if (string.IsNullOrWhiteSpace(sql))
        {
            throw new ArgumentException("Please specify a value for the sql attribute.");
        }
        using var connection = CreateOpenConnection();
        return await connection.ExecuteAsync(sql, parameters, commandTimeout: DefaultTimeout);
    }
}