using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Dapper.CQRS;

/// <summary>
/// Exposes the overloads which execute synchronous and asynchronous commands
/// </summary>
public interface ICommandExecutor
{
    /// <summary>
    /// Execute a synchronous command without a result
    /// </summary>
    /// <param name="command"></param>
    void Execute(Command command);
    
    /// <summary>
    /// Executes a synchronous command and returns the result
    /// </summary>
    /// <param name="command"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T Execute<T>(Command<T> command);
    
    /// <summary>
    /// Executes a asynchronous command without a result
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task ExecuteAsync(CommandAsync command);
    
    /// <summary>
    /// Executes a asynchronous command and returns a result
    /// </summary>
    /// <param name="command"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T> ExecuteAsync<T>(CommandAsync<T> command);
}

/// <summary>
/// Exposes the overloads which execute synchronous and asynchronous commands
/// </summary>
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