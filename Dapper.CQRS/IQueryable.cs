using System;
using System.Collections.Generic;
using System.Data;

namespace Dapper.CQRS
{
    public interface IQueryable
    {
        T QueryFirst<T>(string sql, object parameters = null);
        List<TReturn> QueryList<TFirst, TSecond, TReturn>(
            string sql,
            Func<TFirst, TSecond, TReturn> map,
            object parameters = null);
        List<TReturn> QueryList<TFirst, TSecond, TThird, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TReturn> map, object parameters = null);

        List<T> QueryList<T>(string sql, object parameters = null);
        IDbConnection Db { get; set; }
    }
}