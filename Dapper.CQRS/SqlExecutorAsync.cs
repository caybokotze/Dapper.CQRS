using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dapper.CQRS
{
    public class SqlExecutorAsync
    {
        private ICommandExecutor? _commandExecutor;
        private ILogger? _logger;
        private IDbConnection? _dbConnection;
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
            _dbConnection = serviceProvider.GetRequiredService<IDbConnection>();
            _commandExecutor = serviceProvider.GetRequiredService<ICommandExecutor>();
            _logger = serviceProvider.GetRequiredService<ILogger<SqlExecutor>>();
        }

        private IDbConnection CreateOpenConnection()
        {
            if (_serviceProvider is null)
            {
                throw new InvalidOperationException("The IDbConnection instance is null. Check to see whether this has properly been initialised. The command/query needs to be executed via a IQueryExecutor or ICommandExecutor .Execute method");
            }
            
            var connection =  _serviceProvider.GetRequiredService<IDbConnection>();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }

        public virtual async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null)
        {
            using var connection = CreateOpenConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }
        
        public virtual async Task<T> QueryFirstAsync<T>(string sql, object? parameters = null)
        {
            using var connection = CreateOpenConnection();
            return await connection.QueryFirstAsync<T>(sql, parameters);
        }

        public virtual async Task<IEnumerable<T>> QueryListAsync<T>(string sql, object? parameters = null)
        {
            using var connection = CreateOpenConnection();
            return await connection.QueryAsync<T>(sql, parameters);
        }

        public virtual async Task<IEnumerable<TReturn>> QueryListAsync<T1, T2, TReturn>(string sql,
            Func<T1, T2, TReturn> map,
            object? parameters = null)
        {
            using var connection = CreateOpenConnection();
            return await connection.QueryAsync(sql, map, parameters);
        }

        public virtual async Task<IEnumerable<TReturn>> QueryListAsync<T1, T2, T3, TReturn>(string sql,
            Func<T1, T2, T3, TReturn> map,
            object? parameters = null)
        {
            using var connection = CreateOpenConnection();
            return await connection.QueryAsync(sql, map, parameters);
        }

        public virtual async Task<IEnumerable<TReturn>> QueryListAsync<T1, T2, T3, T4, TReturn>(string sql,
            Func<T1, T2, T3, T4, TReturn> map,
            object? parameters = null)
        {
            using var connection = CreateOpenConnection();
            return await connection.QueryAsync(sql, map, parameters);
        }

        public virtual async Task<IEnumerable<TReturn>> QueryListAsync<T1, T2, T3, T4, T5, TReturn>(string sql,
            Func<T1, T2, T3, T4, T5, TReturn> map,
            object? parameters = null)
        {
            using var connection = CreateOpenConnection();
            return await connection.QueryAsync(sql, map, parameters);
        }

        public virtual async Task<IEnumerable<TReturn>> QueryListAsync<T1, T2, T3, T4, T5, T6, TReturn>(string sql,
            Func<T1, T2, T3, T4, T5, T6, TReturn> map,
            object? parameters = null)
        {
            using var connection = CreateOpenConnection();
            return await connection.QueryAsync(sql, map, parameters);
        }

        public virtual async Task<IEnumerable<TReturn>> QueryListAsync<T1, T2, T3, T4, T5, T6, T7, TReturn>(string sql,
            Func<T1, T2, T3, T4, T5, T6, T7, TReturn> map,
            object? parameters = null)
        {
            using var connection = CreateOpenConnection();
            return await connection.QueryAsync(sql, map, parameters);
        }

        public virtual async Task<int> ExecuteAsync(string sql, object? parameters = null)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException("Please specify a value for the sql attribute.");
            }
            using var connection = CreateOpenConnection();
            return await connection.ExecuteAsync(sql, parameters);
        }
    }
}