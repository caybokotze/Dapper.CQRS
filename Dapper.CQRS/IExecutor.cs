namespace Dapper.CQRS
{
    public interface IExecutor
    {
        int Execute(string sql, object parameters = null);
    }
}