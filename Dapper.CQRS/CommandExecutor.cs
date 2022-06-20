using Microsoft.Extensions.Logging;

namespace Dapper.CQRS
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IExecutable _executable;
        private readonly IQueryable _queryable;
        private readonly ILoggerFactory _loggerFactory;

        public CommandExecutor(IExecutable executable, IQueryable queryable, ILoggerFactory loggerFactory)
        {
            _executable = executable;
            _queryable = queryable;
            _loggerFactory = loggerFactory;
        }
        
        public void Execute(Command command)
        {
            command.Initialise(_executable, _queryable, _loggerFactory);
            command.Execute();
        }

        public T Execute<T>(Command<T> command)
        {
            Execute((Command) command);
            return command.Result;
        }
    }
}