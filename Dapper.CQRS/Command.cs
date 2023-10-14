using System;
using System.Transactions;
using Dapper.CQRS.Exceptions;

namespace Dapper.CQRS
{
    /// <summary>
    /// A synchronous Command which does not expose a result
    /// Exposes a QueryExecutor and CommandExecutor to perform internal operations
    /// </summary>
    public abstract class Command : SqlExecutor
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
        /// A hydrated instance of a CommandExecutor.
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
        public abstract void Execute();
        
        /// <summary>
        /// Throw if there is no defined transaction scope.
        /// </summary>
        /// <exception cref="TransactionScopeRequired"></exception>
        public void ValidateTransactionScope()
        {
            if (Transaction.Current is null)
            {
                throw new TransactionScopeRequired();
            }
        }
    }
    
    /// <summary>
    /// A synchronous Command which exposes a return type of type(T).
    /// Exposes a QueryExecutor and a CommandExecutor to perform internal operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Command<T> : SqlExecutor
    {
       protected Command()
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
        
        private ICommandExecutor? _commandExecutor;
        
        /// <summary>
        /// A hydrated instance of a CommandExecutor.
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
        public abstract T Execute();
        
        /// <summary>
        /// Throw if there is no defined transaction scope.
        /// </summary>
        /// <exception cref="TransactionScopeRequired"></exception>
        public void ValidateTransactionScope()
        {
            if (Transaction.Current is null)
            {
                throw new TransactionScopeRequired();
            }
        }
    } 
    
    
}