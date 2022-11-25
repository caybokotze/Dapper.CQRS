namespace Dapper.CQRS
{
    public abstract class Query<T> : BaseSqlExecutor
    {
        public abstract T Execute();
    }
}