using System;

namespace Dapper.CQRS.Exceptions
{
    public class TransactionScopeRequired : Exception
    {
        public TransactionScopeRequired() : base("The transaction scope has not been defined for the query/command")
        {
            
        }
    }
}