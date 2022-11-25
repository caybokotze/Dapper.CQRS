namespace Dapper.CQRS
{
    public abstract class Command<T> : BaseSqlExecutor
    {
        public abstract T Execute();
    }

    public abstract class Command : BaseSqlExecutor
    {
        public abstract void Execute();
    }
}