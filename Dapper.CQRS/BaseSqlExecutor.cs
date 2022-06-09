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

        private IExecutor Executor { get; set; }
        private IQueryable Queryable { get; set; }
        
        public void Initialise(IExecutor executor, IQueryable queryable, ILogger<BaseSqlExecutor> logger)
        {
            Executor = executor;
            Queryable = queryable;
            Db = queryable.Db;
            Logger = logger;
            QueryExecutor = new QueryExecutor(executor, queryable, logger);
            CommandExecutor = new CommandExecutor(executor, queryable, logger);
        }

        protected ILogger Logger { get; private set; }

        protected T QueryFirst<T>(string sql, object parameters = null)
        {
            return Queryable.QueryFirst<T>(sql, parameters);
        }

        protected IDbConnection Db { get; private set; }

        protected List<TReturn> QueryList<TFirst, TSecond, TReturn>(
            string sql,
            Func<TFirst, TSecond, TReturn> map,
            object parameters = null)
        {
            return Queryable.QueryList(sql, map, parameters);
        }

        protected IEnumerable<TReturn> QueryList<TFirst, TSecond, TThird, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TReturn> map,
            object parameters = null)
        {
            return Queryable.QueryList(sql, map, parameters);
        }

        protected List<T> QueryList<T>(string sql, object parameters = null)
        {
            return Queryable.QueryList<T>(sql, parameters);
        }

        protected int Execute(string sql, object parameters = null)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException("Please specify a value for the sql attribute.");
            }

            return Executor.Execute(sql, parameters);
        }
    }
}