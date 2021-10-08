using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Dapper.CQRS
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IDbConnection _connection;

        public CommandExecutor(IDbConnection connection)
        {
            _connection = connection;
        }
        
        public void Execute(Command command)
        {
            command.InitialiseIDbConnection(_connection);
            command.Execute();
        }

        public T Execute<T>(Command<T> command)
        {
            Execute((Command) command);
            return command.Result;
        }

        public void Execute(IEnumerable<Command> commands)
        {
            if (!(commands is Command[] commandArray))
                commandArray = commands.ToArray();
            
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }
            
            foreach (var t in commandArray)
            {
                t.Execute();
            }
        }
    }
}