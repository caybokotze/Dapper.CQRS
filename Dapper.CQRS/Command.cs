using System;
using System.Threading.Tasks;

namespace Dapper.CQRS
{
    public abstract class Command<T> : Command
    {
        protected Command()
        {
            Result = new ErrorResult<T>("The result was not assigned during the execution of the command. Please check that .Result<T> is set in the inherited .Command<T>");
        }
        
        public Result<T> Result { get; set; }
    }

    public abstract class Command : SqlExecutor
    {
        private IQueryExecutor? _queryExecutor;
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
        
        public abstract void Execute();
    }
    
    
    public abstract class CommandAsync<T> : CommandAsync
    {
        protected CommandAsync()
        {
            Result = new ErrorResult<T>("The result was not assigned during the execution of the command. Please check that .Result<T> is set in the inherited .Command<T>");
        }
        
        public Result<T> Result { get; set; }
    }
    
    public abstract class CommandAsync : SqlExecutorAsync
    {
        private IQueryExecutor? _queryExecutor;
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
        
        public abstract Task ExecuteAsync();
    }
}