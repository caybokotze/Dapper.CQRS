using System;
using System.Threading.Tasks;

namespace Dapper.CQRS
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public async Task Execute(Command command)
        {
            command.Initialise(_serviceProvider);
            await command.Execute();
        }

        public async Task<T> Execute<T>(Command<T> command)
        {
            command.Initialise(_serviceProvider);
            return await command.Execute();
        }
    }
}