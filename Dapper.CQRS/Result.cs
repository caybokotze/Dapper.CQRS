using System;
using System.Collections.Generic;
using Dapper.CQRS.Exceptions;

namespace Dapper.CQRS;

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

public class SuccessResult : Result
{
    public SuccessResult()
    {
        Success = true;
    }
}

public class SuccessResult<T> : Result<T>
{
    public SuccessResult(T data) : base(data)
    {
        Success = true;
    }
}

public class Error
{
    public Error(string details) : this(null, details)
    {
            
    }

    public Error(string? code, string details)
    {
        Code = code;
        Details = details;
    }

    public string? Code { get; set; }
    public string Details { get; set; }
}
    
internal interface IErrorResult
{
    string Message { get; }
    IReadOnlyCollection<Error> Errors { get; }
}

public class ErrorResult : Result, IErrorResult
{
    public ErrorResult(string message) : this(message, Array.Empty<Error>())
    {
            
    }

    public ErrorResult(string message, IReadOnlyCollection<Error> errors)
    {
        Message = message;
        Success = false;
        // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
        Errors = errors ?? ArraySegment<Error>.Empty;
    }
        
    public string Message { get; }
    public IReadOnlyCollection<Error> Errors { get; }
}

public class ErrorResult<T> : Result<T>, IErrorResult
{
    public ErrorResult(string message) : this(message, Array.Empty<Error>())
    {
    }

    public ErrorResult(string message, IReadOnlyCollection<Error> errors) : base(default)
    {
        Message = message;
        Success = false;
        Errors = errors ?? ArraySegment<Error>.Empty;
    }

    public string Message { get; }
    public IReadOnlyCollection<Error> Errors { get; }
}