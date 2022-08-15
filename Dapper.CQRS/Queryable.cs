using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;

namespace Dapper.CQRS
{
    public class Queryable : IQueryable
    {
        private readonly IDbConnection _connection;

        public Queryable(IDbConnection connection)
        {
            _connection = connection;
        }

        public T QueryFirst<T>(string sql, object parameters = null)
        {
            return _connection.QueryFirst<T>(sql, parameters);
        }

        public IList<TReturn> QueryList<TFirst, TSecond, TReturn>(
            string sql, Func<TFirst, TSecond, TReturn> map, 
            object parameters = null)
        {
            return _connection
                .Query(sql, map, parameters)
                .ToList();
        }

        public IList<TReturn> QueryList<TFirst, TSecond, TThird, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TReturn> map, 
            object parameters = null)
        {
            return _connection
                .Query(sql, map, parameters)
                .ToList();
        }

        public IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TReturn>(
            string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, 
            object parameters = null)
        {
            return _connection
                .Query(sql, map, parameters)
                .ToList();
        }

        public IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, 
            object parameters = null)
        {
            return _connection
                .Query(sql, map, parameters)
                .ToList();
        }

        public IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
            string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
            object parameters = null)
        {
            return _connection
                .Query(sql, map, parameters)
                .ToList();
        }

        public IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
            string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
            object parameters = null)
        {
            return _connection
                .Query(sql, map, parameters)
                .ToList();
        }

        public IList<T> QueryList<T>(string sql, object parameters = null)
        {
            return _connection.Query<T>(sql, parameters).ToList();
        }
    }
}