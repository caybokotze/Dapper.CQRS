using System;

namespace Dapper.CQRS.Exceptions
{
    public class TransactionScopeRequiredException : Exception
    {
        public TransactionScopeRequiredException() : base("")
        {
            
        }
    }
}