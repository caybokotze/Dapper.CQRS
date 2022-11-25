namespace Dapper.CQRS.Tests
{
    public class GenericCommand<T> : Command<T>
    {
        private readonly string _sql;
        private readonly object _parameters;
        private readonly T _expectedReturnValue;

        public GenericCommand(T expectedReturnValue)
        {
            _expectedReturnValue = expectedReturnValue;
        }

        public GenericCommand(string sql, object? parameters = null)
        {
            _sql = sql;
            _parameters = parameters;
        }
            
        public override T Execute()
        {
            return _sql != null
                ? QueryFirst<T>(_sql, _parameters) 
                : _expectedReturnValue;
        }
    }
}