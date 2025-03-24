using System;

namespace Dapper.CQRS;

/// <summary>
/// A synchronous Query which not not expose a result
/// Exposes a QueryExecutor to perform internal operations
/// </summary>
public abstract class Query : SqlExecutor
{
    private IQueryExecutor? _queryExecutor;
        
    /// <summary>
    /// A hydrated instance of a QueryExecutor
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public IQueryExecutor QueryExecutor {
        get
        {
            if (_queryExecutor is null)
            {
                throw new InvalidOperationException($"The {nameof(QueryExecutor)} is null. Please check to see whether this query is being executed via the `{nameof(IQueryExecutor)}.Execute()` method");
            }

            return _queryExecutor;
        }
            
        internal set => _queryExecutor = value;
    }
        
    /// <summary>
    /// Execute this instance of 'Query'
    /// </summary>
    /// <returns></returns>
    public abstract void Execute();
}

/// <summary>
/// A synchronous Query which exposes a result
/// Exposes a QueryExecutor to perform internal operations
/// </summary>
public abstract class Query<T> : SqlExecutor
{
    private IQueryExecutor? _queryExecutor;
        
    /// <summary>
    /// A hydrated instance of a QueryExecutor
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public IQueryExecutor QueryExecutor {
        get
        {
            if (_queryExecutor is null)
            {
                throw new InvalidOperationException($"The {nameof(QueryExecutor)} is null. Please check to see whether this query is being executed via the `{nameof(IQueryExecutor)}.Execute()` method");
            }

            return _queryExecutor;
        }
        
        internal set => _queryExecutor = value;
    }
        
    /// <summary>
    /// Execute this instance of 'Query'
    /// </summary>
    /// <returns></returns>
    public abstract T Execute();
}