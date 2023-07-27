using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper.CQRS
{
    public interface ICommandExecutor
    {
        Result Execute(Command command);
        Result<T> Execute<T>(Command<T> command);
        Task<Result> ExecuteAsync(CommandAsync command);
        Task<Result<T>> ExecuteAsync<T>(CommandAsync<T> command);
    }
    
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public Result Execute(Command command)
        {
            // TODO: Implement error handling strategy here
            command.InitialiseExecutor(_serviceProvider);
            try
            {
                command.Execute();
            }
            catch (Exception e)
            {
                return new ErrorResult($"Something went wrong while executing {nameof(command)}", new List<Error>{ new(e.Message) });
            }

            return new SuccessResult();
        }

        public Result<T> Execute<T>(Command<T> command)
        {
            command.InitialiseExecutor(_serviceProvider);
            command.Execute();
            return command.Result;
        }

        public async Task<Result> ExecuteAsync(CommandAsync command)
        {
            command.InitialiseExecutor(_serviceProvider);
            try
            {
                await command.ExecuteAsync();
            }
            catch (Exception e)
            {
                return new ErrorResult($"Something went wrong while executing {nameof(command)}", new List<Error>{ new(e.Message) });
            }

            return new SuccessResult();
        }

        public async Task<Result<T>> ExecuteAsync<T>(CommandAsync<T> command)
        {
            command.InitialiseExecutor(_serviceProvider);
            await command.ExecuteAsync();
            return command.Result;
        }
    }
}