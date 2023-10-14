#nullable enable
using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dapper.CQRS
{
    /// <summary>
    /// A synchronous SqlExecutor which exposes the ServiceProvider (to resolve dependencies) and Dapper methods.
    /// All dapper methods are marked as virtual to allow for mocked return values with Moq or NSubstitute.
    /// </summary>
    public class SqlExecutor
    {
        private ILogger? _logger;
        private IServiceProvider? _serviceProvider;

        protected ILogger Logger => _logger
                                    ?? throw new InvalidOperationException(
                                        "The logger has not been correctly initialised. Make sure this is being executed via a ICommandExecutor / IQueryExecutor");

        protected IDbConnection Connection => CreateOpenConnection();

        protected T GetRequiredService<T>() where T : notnull
        {
            if (_serviceProvider is null)
            {
                throw new InvalidOperationException("The IServiceProvider instance is null. Check to see whether this has properly been initialised. The command/query needs to be executed via a IQueryExecutor or ICommandExecutor .Execute method");
            }

            return _serviceProvider.GetRequiredService<T>();
        }

        internal void InitialiseExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetRequiredService<ILogger<SqlExecutor>>();
        }

        private IDbConnection CreateOpenConnection()
        {
            if (_serviceProvider is null)
            {
                throw new InvalidOperationException("The IDbConnection instance is null. Check to see whether this has properly been initialised. The command/query needs to be executed via a IQueryExecutor or ICommandExecutor .Execute method");
            }
            
            var connection =  _serviceProvider.GetRequiredService<IDbConnection>();
            
            if (Transaction.Current is null && connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }

        public virtual T QueryFirstOrDefault<T>(string sql, object? parameters = null)
        {
            using var connection = CreateOpenConnection();
            return connection.QueryFirstOrDefault<T>(sql, parameters);
        }
        
        public virtual T QueryFirst<T>(string sql, object? parameters = null)
        {
            using var connection = CreateOpenConnection();
            return connection.QueryFirst<T>(sql, parameters);
        }

        public virtual IEnumerable<T> QueryList<T>(string sql, object? parameters = null)
        {
            using var connection = CreateOpenConnection();
            return connection.Query<T>(sql, parameters);
        }

        public virtual IEnumerable<TReturn> QueryList<T1, T2, TReturn>(
            string sql,
            Func<T1, T2, TReturn> map,
            object? parameters = null)
        {
            using var connection = CreateOpenConnection();
            return connection.Query(sql, map, parameters);
        }

        public virtual IEnumerable<TReturn> QueryList<T1, T2, T3, TReturn>(
            string sql,
            Func<T1, T2, T3, TReturn> map,
            object? parameters = null)
        {
            using var connection = CreateOpenConnection();
            return connection.Query(sql, map, parameters);
        }

        public virtual IEnumerable<TReturn> QueryList<T1, T2, T3, T4, TReturn>(
            string sql,
            Func<T1, T2, T3, T4, TReturn> map,
            object? parameters = null)
        {
            using var connection = CreateOpenConnection();
            return connection.Query(sql, map, parameters);
        }

        public virtual IEnumerable<TReturn> QueryList<T1, T2, T3, T4, T5, TReturn>(
            string sql,
            Func<T1, T2, T3, T4, T5, TReturn> map,
            object? parameters = null)
        {
            using var connection = CreateOpenConnection();
            return connection.Query(sql, map, parameters);
        }

        public virtual IEnumerable<TReturn> QueryList<T1, T2, T3, T4, T5, T6, TReturn>(
            string sql,
            Func<T1, T2, T3, T4, T5, T6, TReturn> map,
            object? parameters = null)
        {
            using var connection = CreateOpenConnection();
            return connection.Query(sql, map, parameters);
        }

        public virtual IEnumerable<TReturn> QueryList<T1, T2, T3, T4, T5, T6, T7, TReturn>(
            string sql,
            Func<T1, T2, T3, T4, T5, T6, T7, TReturn> map,
            object? parameters = null)
        {
            using var connection = CreateOpenConnection();
            return connection.Query(sql, map, parameters);
        }

        public virtual int Execute(string sql, object? parameters = null)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException("Please specify a value for the sql attribute.");
            }
            using var connection = CreateOpenConnection();
            return connection.Execute(sql, parameters);
        }
    }
}