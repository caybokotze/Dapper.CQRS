using System;
using System.Collections.Generic;
using System.Data;
using Dapper.CQRS.Interfaces;

namespace Dapper.CQRS
{
    public class BaseSqlExecutor
    {
        public IQueryExecutor QueryExecutor { get; private set; }
        public ICommandExecutor CommandExecutor { get; private set; }
        public void InitialiseDependencies(IBaseSqlExecutorOptions options)
        {
            QueryExecutor = new QueryExecutor(options.ServiceProvider);
            CommandExecutor = new CommandExecutor(options.ServiceProvider);
            _connection = options.Connection;
            Dbms = options.Dbms;
        }

        private IDbConnection _connection;
        public DBMS Dbms { get; set; }

        public T QueryFirst<T>(string sql, object parameters = null)
        {
            return _connection.QueryFirst<T>(sql, parameters);
        }
        
        public List<T> QueryList<T>(string sql, object parameters = null)
        {
            return (List<T>)_connection.Query<T>(sql, parameters);
        }

        public IDbConnection Raw()
        {
            return _connection;
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