namespace Dapper.CQRS
{
    public abstract class Query<T> : Query
    {
        public T Result { get; protected set; }
    }
    
    public abstract class Query : BaseSqlExecutor
    {
        public abstract void Execute();
    }
}