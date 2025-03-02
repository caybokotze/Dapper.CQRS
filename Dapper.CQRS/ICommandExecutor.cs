using System.Threading.Tasks;

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