using System.Collections.Generic;

namespace Dapper.CQRS.Results
{
    public class NotFoundResult<T> : ErrorResult<T>
    {
        public NotFoundResult(string message) : base(message)
        {
        }

        public NotFoundResult(string message, IReadOnlyCollection<Error> errors) : base(message, errors)
        {
        }
    }
}