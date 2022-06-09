using System.Data;
using Microsoft.Extensions.Logging;

namespace Dapper.CQRS
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<BaseSqlExecutor> _logger;

        public CommandExecutor(IDbConnection connection, ILogger<BaseSqlExecutor> logger)
        {
            _connection = connection;
            _logger = logger;
        }
        
        public void Execute(Command command)
        {
            command.Initialise(_connection, _logger);
            command.Execute();
        }

        public T Execute<T>(Command<T> command)
        {
            Execute((Command) command);
            return command.Result;
        }
    }
}