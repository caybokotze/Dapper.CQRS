using System;

namespace Dapper.CQRS
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public void Execute(Command command)
        {
            command.Initialise(_serviceProvider);
            command.Execute();
        }

        public T Execute<T>(Command<T> command)
        {
            command.Initialise(_serviceProvider);
            return command.Execute();
        }
    }
}