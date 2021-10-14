using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Dapper.CQRS
{
    public class BaseSqlExecutor
    {
        public IQueryExecutor QueryExecutor { get; private set; }
        public ICommandExecutor CommandExecutor { get; private set; }
        
        public void InitialiseIDbConnection(IDbConnection connection)
        {
            QueryExecutor = new QueryExecutor(connection);
            CommandExecutor = new CommandExecutor(connection);
            _connection = connection;
        }

        private IDbConnection _connection;

        public T QueryFirst<T>(string sql, object parameters = null)
        {
            return _connection.QueryFirst<T>(sql, parameters);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(
            string sql,
            Func<TFirst, TSecond, TReturn> map,
            object parameters = null)
        {
            return _connection.Query<TFirst, TSecond, TReturn>(sql, map, parameters);
        }
        
        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TReturn> map,
            object parameters = null)
        {
            return _connection.Query<TFirst, TSecond, TThird, TReturn>(sql, map, parameters);
        }

        public List<T> QueryList<T>(string sql, object parameters = null)
        {
            return _connection.Query<T>(sql, parameters).ToList();
        }

        protected int Execute(string sql, object parameters = null)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException("Please specify a value for the sql attribute.");
            }
            
            return _connection.Execute(sql, parameters);
        }
    }
}