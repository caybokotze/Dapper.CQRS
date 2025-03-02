using System;
using System.Transactions;
using Dapper.CQRS.Exceptions;

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

    /// <summary>
    /// Throw if there is no defined transaction scope.
    /// </summary>
    /// <exception cref="TransactionScopeRequired"></exception>
    public virtual void ValidateTransactionScope()
    {
        if (Transaction.Current is null)
        {
            throw new TransactionScopeRequired();
        }
    }
}

/// <summary>
/// A synchronous Query which exposes a result
/// Exposes a QueryExecutor to perform internal operations
/// </summary>
public abstract class Query<T> : SqlExecutor
{
    protected Query()
    {
    }
        
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

    /// <summary>
    /// Default implementation for .Validate checks whether a transaction scope was set for the current query instance.
    /// </summary>
    /// <exception cref="TransactionScopeRequired"></exception>
    public virtual void ValidateTransactionScope()
    {
        if (Transaction.Current is null)
        {
            throw new TransactionScopeRequired();
        }
    }
}