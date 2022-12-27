using System.Threading.Tasks;

namespace Dapper.CQRS
{
    public abstract class Query<T> : SqlExecutor
    {
        public abstract Task<T> Execute();
    }
}