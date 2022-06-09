using System.Data;
using Microsoft.Extensions.Logging;

namespace Dapper.CQRS
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IExecutor _executor;
        private readonly IQueryable _queryable;
        private readonly ILogger<BaseSqlExecutor> _logger;

        public CommandExecutor(IExecutor executor, IQueryable queryable, ILogger<BaseSqlExecutor> logger)
        {
            _executor = executor;
            _queryable = queryable;
            _logger = logger;
        }
        
        public void Execute(Command command)
        {
            command.Initialise(_executor, _queryable, _logger);
            command.Execute();
        }

        public T Execute<T>(Command<T> command)
        {
            Execute((Command) command);
            return command.Result;
        }
    }
}