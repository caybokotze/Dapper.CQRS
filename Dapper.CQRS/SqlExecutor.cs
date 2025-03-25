#nullable enable
using System;
using System.Collections.Generic;
using System.Transactions;

namespace Dapper.CQRS;

/// <summary>
/// A synchronous SqlExecutor which exposes the ServiceProvider (to resolve dependencies) and Dapper methods.
/// All dapper methods are marked as virtual to allow for mocked return values with Moq or NSubstitute.
/// </summary>
public class SqlExecutor : BaseConnection
{
    public virtual T? QueryFirstOrDefault<T>(string sql, object? parameters = null)
    {
        if (ConnectionConfiguration.CreateTransaction)
        {
            using var transaction = new TransactionScope(
                ConnectionConfiguration.DefaultTransactionScopeOption, 
                new TransactionOptions
            {
                IsolationLevel = ConnectionConfiguration.DefaultIsolationLevel,
                Timeout = TimeSpan.FromSeconds(Configuration.ScopedTimeout)
            });
            
            using var scopedConnection = CreateOpenConnection();
            var result = scopedConnection.QueryFirstOrDefault<T>(sql, parameters, commandTimeout: Configuration.ScopedTimeout);
            
            if (!ConnectionConfiguration.AutoRollback)
            {
                transaction.Complete();
            }
            
            return result;
        }

        using var connection = CreateOpenConnection();
        return connection.QueryFirstOrDefault<T>(sql, parameters, commandTimeout: Configuration.ScopedTimeout);
    }
        
    public virtual T QueryFirst<T>(string sql, object? parameters = null)
    {
        if (ConnectionConfiguration.CreateTransaction)
        {
            using var transaction = new TransactionScope(
                ConnectionConfiguration.DefaultTransactionScopeOption, 
                new TransactionOptions
            {
                IsolationLevel = ConnectionConfiguration.DefaultIsolationLevel,
                Timeout = TimeSpan.FromSeconds(Configuration.ScopedTimeout)
            });
            
            using var scopedConnection = CreateOpenConnection();
            var result = scopedConnection.QueryFirst<T>(sql, parameters, commandTimeout: Configuration.ScopedTimeout);
            
            if (!ConnectionConfiguration.AutoRollback)
            {
                transaction.Complete();
            }
            return result;
        }

        using var connection = CreateOpenConnection();
        return connection.QueryFirst<T>(sql, parameters, commandTimeout: Configuration.ScopedTimeout);
    }

    public virtual IEnumerable<T> QueryList<T>(string sql, object? parameters = null)
    {
        if (ConnectionConfiguration.CreateTransaction)
        {
            using var transaction = new TransactionScope(
                ConnectionConfiguration.DefaultTransactionScopeOption, 
                new TransactionOptions
            {
                IsolationLevel = ConnectionConfiguration.DefaultIsolationLevel,
                Timeout = TimeSpan.FromSeconds(Configuration.ScopedTimeout)
            });
            
            using var scopedConnection = CreateOpenConnection();
            var result = scopedConnection.Query<T>(sql, parameters, commandTimeout: Configuration.ScopedTimeout);
            
            if (!ConnectionConfiguration.AutoRollback)
            {
                transaction.Complete();
            }
            return result;
        }
        
        using var connection = CreateOpenConnection();
        return connection.Query<T>(sql, parameters, commandTimeout: Configuration.ScopedTimeout);
    }

    public virtual IEnumerable<TReturn> QueryList<T1, T2, TReturn>(
        string sql,
        Func<T1, T2, TReturn> map,
        object? parameters = null)
    {
        if (ConnectionConfiguration.CreateTransaction)
        {
            using var transaction = new TransactionScope(
                ConnectionConfiguration.DefaultTransactionScopeOption, 
                new TransactionOptions
            {
                IsolationLevel = ConnectionConfiguration.DefaultIsolationLevel,
                Timeout = TimeSpan.FromSeconds(Configuration.ScopedTimeout)
            });
            
            using var scopedConnection = CreateOpenConnection();
            var result = scopedConnection.Query(sql, map, parameters, splitOn: Configuration.ScopedSplitOn, commandTimeout: Configuration.ScopedTimeout);

            if (!ConnectionConfiguration.AutoRollback)
            {
                transaction.Complete();
            }
            
            return result;
        }
        
        using var connection = CreateOpenConnection();
        return connection.Query(sql, map, parameters, splitOn: Configuration.ScopedSplitOn, commandTimeout: Configuration.ScopedTimeout);
    }

    public virtual IEnumerable<TReturn> QueryList<T1, T2, T3, TReturn>(
        string sql,
        Func<T1, T2, T3, TReturn> map,
        object? parameters = null)
    {
        if (ConnectionConfiguration.CreateTransaction)
        {
            using var transaction = new TransactionScope(
                ConnectionConfiguration.DefaultTransactionScopeOption, 
                new TransactionOptions
                {
                    IsolationLevel = ConnectionConfiguration.DefaultIsolationLevel,
                    Timeout = TimeSpan.FromSeconds(Configuration.ScopedTimeout)
                });
            
            using var scopedConnection = CreateOpenConnection();
            var result = scopedConnection.Query(sql, map, parameters, splitOn: Configuration.ScopedSplitOn, commandTimeout: Configuration.ScopedTimeout);
            
            if (!ConnectionConfiguration.AutoRollback)
            {
                transaction.Complete();
            }
            
            return result;
        }
        
        using var connection = CreateOpenConnection();
        return connection.Query(sql, map, parameters, splitOn: Configuration.ScopedSplitOn, commandTimeout: Configuration.ScopedTimeout);
    }

    public virtual IEnumerable<TReturn> QueryList<T1, T2, T3, T4, TReturn>(
        string sql,
        Func<T1, T2, T3, T4, TReturn> map,
        object? parameters = null)
    {
        if (ConnectionConfiguration.CreateTransaction)
        {
            using var transaction = new TransactionScope(
                ConnectionConfiguration.DefaultTransactionScopeOption, 
                new TransactionOptions
            {
                IsolationLevel = ConnectionConfiguration.DefaultIsolationLevel,
                Timeout = TimeSpan.FromSeconds(Configuration.ScopedTimeout)
            });
            
            using var scopedConnection = CreateOpenConnection();
            var result = scopedConnection.Query(sql, map, parameters, splitOn: Configuration.ScopedSplitOn, commandTimeout: Configuration.ScopedTimeout);
            
            if (!ConnectionConfiguration.AutoRollback)
            {
                transaction.Complete();
            }
            return result;
        }
        
        using var connection = CreateOpenConnection();
        return connection.Query(sql, map, parameters, splitOn: Configuration.ScopedSplitOn, commandTimeout: Configuration.ScopedTimeout);
    }

    public virtual IEnumerable<TReturn> QueryList<T1, T2, T3, T4, T5, TReturn>(
        string sql,
        Func<T1, T2, T3, T4, T5, TReturn> map,
        object? parameters = null)
    {
        if (ConnectionConfiguration.CreateTransaction)
        {
            using var transaction = new TransactionScope(
                ConnectionConfiguration.DefaultTransactionScopeOption,
                new TransactionOptions
                {
                    IsolationLevel = ConnectionConfiguration.DefaultIsolationLevel,
                    Timeout = TimeSpan.FromSeconds(Configuration.ScopedTimeout)
                });
            
            using var scopedConnection = CreateOpenConnection();
            var result = scopedConnection.Query(sql, map, parameters, splitOn: Configuration.ScopedSplitOn, commandTimeout: Configuration.ScopedTimeout);
            
            if (!ConnectionConfiguration.AutoRollback)
            {
                transaction.Complete();
            }
            
            return result;
        }
        
        using var connection = CreateOpenConnection();
        return connection.Query(sql, map, parameters, splitOn: Configuration.ScopedSplitOn, commandTimeout: Configuration.ScopedTimeout);
    }

    public virtual IEnumerable<TReturn> QueryList<T1, T2, T3, T4, T5, T6, TReturn>(
        string sql,
        Func<T1, T2, T3, T4, T5, T6, TReturn> map,
        object? parameters = null)
    {
        if (ConnectionConfiguration.CreateTransaction)
        {
            using var transaction = new TransactionScope(
                ConnectionConfiguration.DefaultTransactionScopeOption, 
                new TransactionOptions
            {
                IsolationLevel = ConnectionConfiguration.DefaultIsolationLevel,
                Timeout = TimeSpan.FromSeconds(Configuration.ScopedTimeout)
            });
            
            using var scopedConnection = CreateOpenConnection();
            var result = scopedConnection.Query(sql, map, parameters, splitOn: Configuration.ScopedSplitOn, commandTimeout: Configuration.ScopedTimeout);
            
            if (!ConnectionConfiguration.AutoRollback)
            {
                transaction.Complete();
            }
            
            return result;
        }
        
        using var connection = CreateOpenConnection();
        return connection.Query(sql, map, parameters, splitOn: Configuration.ScopedSplitOn, commandTimeout: Configuration.ScopedTimeout);
    }

    public virtual IEnumerable<TReturn> QueryList<T1, T2, T3, T4, T5, T6, T7, TReturn>(
        string sql,
        Func<T1, T2, T3, T4, T5, T6, T7, TReturn> map,
        object? parameters = null)
    {
        if (ConnectionConfiguration.CreateTransaction)
        {
            using var transaction = new TransactionScope(
                ConnectionConfiguration.DefaultTransactionScopeOption, 
                new TransactionOptions
            {
                IsolationLevel = ConnectionConfiguration.DefaultIsolationLevel,
                Timeout = TimeSpan.FromSeconds(Configuration.ScopedTimeout)
            });
            
            using var scopedConnection = CreateOpenConnection();
            var result = scopedConnection.Query(sql, map, parameters, splitOn: Configuration.ScopedSplitOn, commandTimeout: Configuration.ScopedTimeout);
            
            if (!ConnectionConfiguration.AutoRollback)
            {
                transaction.Complete();
            }
            
            return result;
        }
        
        using var connection = CreateOpenConnection();
        return connection.Query(sql, map, parameters, splitOn: Configuration.ScopedSplitOn, commandTimeout: Configuration.ScopedTimeout);
    }

    public virtual int Execute(string sql, object? parameters = null)
    {
        if (string.IsNullOrWhiteSpace(sql))
        {
            throw new ArgumentException("Please specify a value for the sql attribute.");
        }

        if (ConnectionConfiguration.CreateTransaction)
        {
            using var transaction = new TransactionScope(
                ConnectionConfiguration.DefaultTransactionScopeOption, 
                new TransactionOptions
            {
                IsolationLevel = ConnectionConfiguration.DefaultIsolationLevel,
                Timeout = TimeSpan.FromSeconds(Configuration.ScopedTimeout)
            });
            
            using var scopedConnection = CreateOpenConnection();
            var result = scopedConnection.Execute(sql, parameters, commandTimeout: Configuration.ScopedTimeout);
            
            if (!ConnectionConfiguration.AutoRollback)
            {
                transaction.Complete();
            }
            
            return result;
        }
        
        using var connection = CreateOpenConnection();
        return connection.Execute(sql, parameters, commandTimeout: Configuration.ScopedTimeout);
    }
}