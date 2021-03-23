using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using DapperDoodle.Interfaces;

namespace DapperDoodle
{
    public class BaseSqlExecutor
    {
        public void InitialiseDependencies(IBaseSqlExecutorOptions options)
        {
            Provider = options.ServiceProvider;
            _connection = options.Connection;
            Dbms = options.Dbms;
        }

        public IServiceProvider Provider;

        private IDbConnection _connection;
        public DBMS Dbms { get; set; }

        protected T QueryFirst<T>(string sql, object parameters = null)
        {
            return _connection.QueryFirst<T>(sql, parameters);
        }
        
        protected List<T> QueryList<T>(string sql, object parameters = null)
        {
            return (List<T>)_connection.Query<T>(sql, parameters);
        }

        public IDbConnection GetIDbConnection()
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