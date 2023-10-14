using System;

namespace Dapper.CQRS.Exceptions;

public class InvalidResult : Exception
{
    public InvalidResult(string message) : base(message)
    {
            
    }
}