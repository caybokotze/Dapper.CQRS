using Dapper.CQRS.Exceptions;

namespace Dapper.CQRS.Results;

public abstract class Result
{
    public bool Success { get; protected set; }
    public bool Failure => !Success;
}

public abstract class Result<T> : Result
{
    private readonly T? _data;

    protected Result(T? data)
    {
        if (data is null)
        { 
            Success = false;
        }
            
        _data = data;
    }
        
    public T Value => (Success ? _data 
        : throw new InvalidResult($"Access to .{nameof(Value)} is not allowed when .{nameof(Success)} is false"))!;
}