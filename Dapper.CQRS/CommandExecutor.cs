using System.Threading.Tasks;

namespace Dapper.CQRS;

/// <summary>
/// Exposes the overloads which execute synchronous and asynchronous commands
/// </summary>
public class CommandExecutor : ICommandExecutor
{
    private readonly IQueryExecutor _queryExecutor;

    public CommandExecutor(IQueryExecutor queryExecutor)
    {
        _queryExecutor = queryExecutor;
    }
        
    public void Execute(Command command)
    {
        command.CommandExecutor = this;
        command.QueryExecutor = _queryExecutor;
        command.Execute();
    }

    public T Execute<T>(Command<T> command)
    {
        command.CommandExecutor = this;
        command.QueryExecutor = _queryExecutor;
        return command.Execute();
    }

    public async Task ExecuteAsync(CommandAsync command)
    {
        command.CommandExecutor = this;
     
        command.QueryExecutor = _queryExecutor;
        await command.ExecuteAsync();
    }

    public async Task<T> ExecuteAsync<T>(CommandAsync<T> command)
    {
        command.CommandExecutor = this;
        command.QueryExecutor = _queryExecutor;
        return await command.ExecuteAsync();
    }
}