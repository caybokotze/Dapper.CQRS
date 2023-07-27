using System;

namespace Dapper.CQRS.Exceptions
{
    public class InvalidSqlStatement : ArgumentException
    {
        public InvalidSqlStatement() : base("Please provide a valid sql argument as a parameter.")
        {
        }
    }
}