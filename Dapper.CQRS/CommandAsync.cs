using System;
using System.Threading.Tasks;

namespace Dapper.CQRS;

/// <summary>
/// A asynchronous Command which does not expose a result
/// Exposes a QueryExecutor and CommandExecutor to perform internal operations
/// </summary>
public abstract class CommandAsync : SqlExecutorAsync
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
        
    private ICommandExecutor? _commandExecutor;
        
    /// <summary>
    /// A hydrated instance of a CommandExecutor
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public ICommandExecutor CommandExecutor {
        get
        {
            if (_commandExecutor is null)
            {
                throw new InvalidOperationException($"The {nameof(CommandExecutor)} is null. Please check to see whether this command is being executed via the `{nameof(ICommandExecutor)}.Execute()` method");
            }

            return _commandExecutor;
        }
            
        internal set => _commandExecutor = value;
    }
        
    /// <summary>
    /// Executes the instance of this 'Command'
    /// </summary>
    /// <returns></returns>
    public abstract Task ExecuteAsync();
}
    
/// <summary>
/// A asynchronous Command which exposes a result
/// Exposes a QueryExecutor and CommandExecutor to perform internal operations
/// </summary>
public abstract class CommandAsync<T> : SqlExecutorAsync
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
        
    private ICommandExecutor? _commandExecutor;
        
    /// <summary>
    /// A hydrated instance of a CommandExecutor
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public ICommandExecutor CommandExecutor {
        get
        {
            if (_commandExecutor is null)
            {
                throw new InvalidOperationException($"The {nameof(CommandExecutor)} is null. Please check to see whether this command is being executed via the `{nameof(ICommandExecutor)}.Execute()` method");
            }

            return _commandExecutor;
        }
            
        internal set => _commandExecutor = value;
    }
        
    /// <summary>
    /// Executes the instance of this 'Command'
    /// </summary>
    /// <returns></returns>
    public abstract Task<T> ExecuteAsync();
}