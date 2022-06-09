namespace Dapper.CQRS.Tests
{
    public class GenericQuery<T> : Query<T>
    {
        private readonly T _expectedValue;
        private readonly string _sql;
        private readonly object _parameters;

        public GenericQuery(T expectedValue)
        {
            _expectedValue = expectedValue;
        }

        public GenericQuery(string sql, object parameters = null)
        {
            _sql = sql;
            _parameters = parameters;
        }
            
        public override void Execute()
        {
            if (_expectedValue is null)
            {
                Result = QueryFirst<T>(_sql, _parameters);
                return;
            }
            
            Result = _expectedValue;
        }
    }
}