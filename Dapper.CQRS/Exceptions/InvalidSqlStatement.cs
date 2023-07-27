using System;

namespace Dapper.CQRS.Exceptions
{
    public class InvalidSqlStatementException : ArgumentException
    {
        public InvalidSqlStatementException() : base("Please provide a valid sql argument as a parameter.")
        {
        }
    }
}