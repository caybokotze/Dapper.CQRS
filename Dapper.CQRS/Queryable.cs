using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Dapper.CQRS
{
    public class Queryable : IQueryable
    {
        private readonly IDbConnection _connection;

        public Queryable(IDbConnection connection)
        {
            Db = connection;
            _connection = connection;
        }

        public T QueryFirst<T>(string sql, object parameters = null)
        {
            return _connection.QueryFirst<T>(sql, parameters);
        }

        public List<TReturn> QueryList<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object parameters = null)
        {
            return _connection
                .Query(sql, map, parameters)
                .ToList();
        }

        public List<TReturn> QueryList<TFirst, TSecond, TThird, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TReturn> map, object parameters = null)
        {
            return _connection
                .Query(sql, map, parameters)
                .ToList();
        }

        public List<T> QueryList<T>(string sql, object parameters = null)
        {
            return _connection.Query<T>(sql, parameters).ToList();
        }
        
        public IDbConnection Db { get; set; }
    }
}