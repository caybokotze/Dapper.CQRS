using System;
using System.Collections.Generic;
using System.Data;

namespace Dapper.CQRS
{
    public interface IQueryable
    {
        T QueryFirst<T>(string sql, object parameters = null);
        IList<TReturn> QueryList<TFirst, TSecond, TReturn>(
            string sql,
            Func<TFirst, TSecond, TReturn> map,
            object parameters = null);
        IList<TReturn> QueryList<TFirst, TSecond, TThird, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TReturn> map, object parameters = null);
        
        IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object parameters = null);
        
        IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object parameters = null);
        
        IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object parameters = null);
        
        IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object parameters = null);

        IList<T> QueryList<T>(string sql, object parameters = null);
    }
}