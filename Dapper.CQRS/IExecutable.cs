namespace Dapper.CQRS
{
    public interface IExecutable
    {
        int Execute(string sql, object parameters = null);
    }
}