using System.Threading.Tasks;

namespace Dapper.CQRS
{
    public interface ICommandExecutor
    {
        Task Execute(Command command);
        Task<T> Execute<T>(Command<T> command);
    }
}