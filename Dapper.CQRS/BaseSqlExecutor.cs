using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Dapper.CQRS
{
    public class BaseSqlExecutor
    {
        public IQueryExecutor QueryExecutor { get; set; }
        public ICommandExecutor CommandExecutor { get; set; }
        
        public void Initialise(IDbConnection connection, ILogger<BaseSqlExecutor> logger)
        {
            
            QueryExecutor = new QueryExecutor(connection, logger);
            CommandExecutor = new CommandExecutor(connection, logger);
            Db = connection;
            Logger = logger;
        }

        protected ILogger Logger { get; private set; }

        protected T QueryFirst<T>(string sql, object parameters = null)
        {
            return Db.QueryFirst<T>(sql, parameters);
        }

        protected IDbConnection Db { get; private set; }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(
            string sql,
            Func<TFirst, TSecond, TReturn> map,
            object parameters = null)
        {
            return Db.Query<TFirst, TSecond, TReturn>(sql, map, parameters);
        }

        protected IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TReturn> map,
            object parameters = null)
        {
            return Db.Query(sql, map, parameters);
        }

        public List<T> QueryList<T>(string sql, object parameters = null)
        {
            return Db.Query<T>(sql, parameters).ToList();
        }

        protected int Execute(string sql, object parameters = null)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException("Please specify a value for the sql attribute.");
            }
            
            return Db.Execute(sql, parameters);
        }
    }
}