namespace Dapper.CQRS.Tests;

public class GenericCommand<T> : Command<T>
{
    private readonly string _sql;
    private readonly object? _parameters;
    private readonly T? _mockedReturnValue;

    public GenericCommand(T mockedReturnValue)
    {
        _sql = string.Empty;
        _mockedReturnValue = mockedReturnValue;
    }

    public GenericCommand(string sql, object? parameters = null)
    {
        _sql = sql;
        _parameters = parameters;
    }
            
    public override T Execute()
    {
        return Connection.QueryFirst<T>(_sql, _parameters);
    }
}