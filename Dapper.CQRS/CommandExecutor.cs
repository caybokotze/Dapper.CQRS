using System;
using System.Collections.Generic;
using System.Linq;
using Dapper.CQRS.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.CQRS
{
    public class CommandExecutor : ICommandExecutor
    {
        public CQRSSqlExecutorOptions Options { get; }

        public CommandExecutor(IServiceProvider provider)
        {
            Options = provider.GetService<CQRSSqlExecutorOptions>();
        }
        
        public void Execute(Command command)
        {
            command.InitialiseDependencies(Options);
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