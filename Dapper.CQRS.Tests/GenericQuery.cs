using System.Threading.Tasks;

namespace Dapper.CQRS.Tests
{
    public class GenericQuery<T> : Query<T>
    {
        private readonly T? _expectedValue;
        private readonly string? _sql;
        private readonly object? _parameters;

        public GenericQuery(T expectedValue)
        {
            _expectedValue = expectedValue;
        }

        public GenericQuery(string sql, object? parameters = null)
        {
            _sql = sql;
            _parameters = parameters;
        }
            
        public override void Execute()
        {
            var result = QueryFirst<T>(_sql ?? string.Empty, _parameters);
            Result = new SuccessResult<T>(result);
        }
    }
}