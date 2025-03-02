using System;
using System.Collections.Generic;

namespace Dapper.CQRS.Results;


internal interface IErrorResult
{
    string Message { get; }
    IReadOnlyCollection<Error> Errors { get; }
}

public class ErrorResult<T> : Result<T>, IErrorResult
{
    public ErrorResult(string message) : this(message, Array.Empty<Error>())
    {
    }

    public ErrorResult(string message, IReadOnlyCollection<Error>? errors) : base(default)
    {
        Message = message;
        Success = false;
        Errors = errors ?? ArraySegment<Error>.Empty;
    }

    public string Message { get; }
    public IReadOnlyCollection<Error> Errors { get; }
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