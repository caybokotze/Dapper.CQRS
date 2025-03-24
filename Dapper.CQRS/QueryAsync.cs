using System;
using System.Threading.Tasks;

namespace Dapper.CQRS;

/// <summary>
/// A asynchronous Query which not not expose a result
/// Exposes a QueryExecutor to perform internal operations
/// </summary>
public abstract class QueryAsync : SqlExecutorAsync
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
    public abstract Task ExecuteAsync();
}
    

/// <summary>
/// A asynchronous Query which exposes a result
/// Exposes a QueryExecutor to perform internal operations
/// </summary>
public abstract class QueryAsync<T> : SqlExecutorAsync 
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
    /// Execute the instance of this 'Query'
    /// </summary>
    /// <returns></returns>
    public abstract Task<T> ExecuteAsync();
}