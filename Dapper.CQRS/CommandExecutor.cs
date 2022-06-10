using System.Data;
using Microsoft.Extensions.Logging;

namespace Dapper.CQRS
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IExecutable _executable;
        private readonly IQueryable _queryable;
        private readonly ILogger<BaseSqlExecutor> _logger;

        public CommandExecutor(IExecutable executable, IQueryable queryable, ILogger<BaseSqlExecutor> logger)
        {
            _executable = executable;
            _queryable = queryable;
            _logger = logger;
        }
        
        public void Execute(Command command)
        {
            command.Initialise(_executable, _queryable, _logger);
            command.Execute();
        }

        public T Execute<T>(Command<T> command)
        {
            Execute((Command) command);
            return command.Result;
        }
    }
}