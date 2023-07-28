using System;
using System.Threading.Tasks;
using System.Transactions;
using Dapper.CQRS.Exceptions;

namespace Dapper.CQRS
{
    public abstract class Query : SqlExecutor
    {
        private IQueryExecutor? _queryExecutor;
        public IQueryExecutor QueryExecutor {
            get
            {
                if (_queryExecutor is null)
                {
                    throw new InvalidOperationException($"The {nameof(QueryExecutor)} is null. Please check to see whether this query is being executed via the `{nameof(IQueryExecutor)}.Execute()` method");
                }

                return _queryExecutor;
            }
            set => _queryExecutor = value;
        }
        
        /// <summary>
        /// Execute this instance of 'Query'
        /// </summary>
        /// <returns></returns>
        public abstract void Execute();
        

        public void ValidateTransactionScope()
        {
            if (Transaction.Current is null)
            {
                throw new TransactionScopeRequired();
            }
        }
    }

    public abstract class Query<T> : Query
    {
        protected Query()
        {
            Result = new ErrorResult<T>("The result was not assigned during the execution of the query. Please check that .Result<T> is set in the inherited .Query<T>");
        }
        
        public Result<T> Result { get; set; }
    }

    public abstract class QueryAsync : SqlExecutorAsync
    {
        private IQueryExecutor? _queryExecutor;
        public IQueryExecutor QueryExecutor {
            get
            {
                if (_queryExecutor is null)
                {
                    throw new InvalidOperationException($"The {nameof(QueryExecutor)} is null. Please check to see whether this query is being executed via the `{nameof(IQueryExecutor)}.Execute()` method");
                }

                return _queryExecutor;
            }
            
            internal set => _queryExecutor = value;
        }
        
        /// <summary>
        /// Execute this instance of 'Query'
        /// </summary>
        /// <returns></returns>
        public abstract Task ExecuteAsync();
        
        public void ValidateTransactionScope()
        {
            if (Transaction.Current is null)
            {
                throw new TransactionScopeRequired();
            }
        }
    }

    public abstract class QueryAsync<T> : QueryAsync
    {
        public QueryAsync()
        {
            Result = new ErrorResult<T>("The result was not assigned during the execution of the query. Please check that .Result<T> is set in the inherited .Query<T>");
        }
        
        public Result<T> Result { get; set; }
    }
}