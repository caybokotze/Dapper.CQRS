using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Logging;

namespace Dapper.CQRS
{
    public class BaseSqlExecutor
    {
        protected IQueryExecutor QueryExecutor { get; set; }
        protected ICommandExecutor CommandExecutor { get; set; }

        private IExecutable Executable { get; set; }
        private IQueryable Queryable { get; set; }
        
        public void Initialise(IExecutable executable, IQueryable queryable, ILoggerFactory loggerFactory)
        {
            Executable = executable;
            Queryable = queryable;
            Logger = loggerFactory.CreateLogger<BaseSqlExecutor>();
            QueryExecutor = new QueryExecutor(executable, queryable, loggerFactory);
            CommandExecutor = new CommandExecutor(executable, queryable, loggerFactory);
        }

        protected ILogger Logger { get; set; }

        protected T QueryFirst<T>(string sql, object parameters = null)
        {
            return Queryable.QueryFirst<T>(sql, parameters);
        }

        protected IList<TReturn> QueryList<TFirst, TSecond, TReturn>(
            string sql,
            Func<TFirst, TSecond, TReturn> map,
            object parameters = null)
        {
            return Queryable.QueryList(sql, map, parameters);
        }

        protected IList<TReturn> QueryList<TFirst, TSecond, TThird, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TReturn> map,
            object parameters = null)
        {
            return Queryable.QueryList(sql, map, parameters);
        }
        
        protected IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map,
            object parameters = null)
        {
            return Queryable.QueryList(sql, map, parameters);
        }
        
        protected IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map,
            object parameters = null)
        {
            return Queryable.QueryList(sql, map, parameters);
        }
        
        protected IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
            object parameters = null)
        {
            return Queryable.QueryList(sql, map, parameters);
        }
        
        protected IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
            object parameters = null)
        {
            return Queryable.QueryList(sql, map, parameters);
        }

        protected IList<T> QueryList<T>(string sql, object parameters = null)
        {
            return Queryable.QueryList<T>(sql, parameters);
        }

        protected int Execute(string sql, object parameters = null)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException("Please specify a value for the sql attribute.");
            }

            return Executable.Execute(sql, parameters);
        }
    }
}