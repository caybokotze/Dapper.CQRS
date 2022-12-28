using System.Threading.Tasks;

namespace Dapper.CQRS
{
    public abstract class Command<T> : SqlExecutor
    {
        public abstract Task<T> Execute();
    }

    public abstract class Command : SqlExecutor
    {
        public abstract Task Execute();
    }
}