using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.CQRS;

public interface ICommandExecutor
{
    void Execute(Command command);
    T Execute<T>(Command<T> command);
    Task ExecuteAsync(CommandAsync command);
    Task<T> ExecuteAsync<T>(CommandAsync<T> command);
}
    
public class CommandExecutor : ICommandExecutor
{
    private readonly IServiceProvider _serviceProvider;

    public CommandExecutor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
        
    public void Execute(Command command)
    {
        command.CommandExecutor = this;
        command.QueryExecutor = _serviceProvider.GetRequiredService<IQueryExecutor>();
        command.InitialiseExecutor(_serviceProvider);
        command.Execute();
    }

    public T Execute<T>(Command<T> command)
    {
        command.CommandExecutor = this;
        command.QueryExecutor = _serviceProvider.GetRequiredService<IQueryExecutor>();
        command.InitialiseExecutor(_serviceProvider);
        return command.Execute();
    }

    public async Task ExecuteAsync(CommandAsync command)
    {
        command.CommandExecutor = this;
        command.QueryExecutor = _serviceProvider.GetRequiredService<IQueryExecutor>();
        command.InitialiseExecutor(_serviceProvider);
        await command.ExecuteAsync();
    }

    public async Task<T> ExecuteAsync<T>(CommandAsync<T> command)
    {
        command.CommandExecutor = this;
        command.QueryExecutor = _serviceProvider.GetRequiredService<IQueryExecutor>();
        command.InitialiseExecutor(_serviceProvider);
        return await command.ExecuteAsync();
    }
}