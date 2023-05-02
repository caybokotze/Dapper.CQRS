#nullable enable
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dapper.CQRS
{
    public interface ISqlExecutor
    {
        void Initialise(IServiceProvider serviceProvider);

        Task<T> QueryFirst<T>(string sql, object? parameters = null);

        Task<IList<TReturn>> QueryList<TFirst, TSecond, TReturn>(
            string sql,
            Func<TFirst, TSecond, TReturn> map,
            object? parameters = null);

        Task<IList<TReturn>> QueryList<TFirst, TSecond, TThird, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TReturn> map,
            object? parameters = null);

        Task<IList<TReturn>> QueryList<TFirst, TSecond, TThird, TFourth, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map,
            object? parameters = null);

        Task<IList<TReturn>> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map,
            object? parameters = null);

        Task<IList<TReturn>> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
            object? parameters = null);

        Task<IList<TReturn>> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
            object? parameters = null);

        Task<IList<T>> QueryList<T>(string sql, object? parameters = null);
        int Execute(string sql, object? parameters = null);
    }

    public class SqlExecutor : ISqlExecutor
    {
        private IQueryExecutor? _queryExecutor;
        private ICommandExecutor? _commandExecutor;
        private ILogger? _logger;
        private IDbConnection? _dbConnection;
        private IServiceProvider? _serviceProvider;

        protected IQueryExecutor QueryExecutor => _queryExecutor
                                                  ?? throw new NullReferenceException(
                                                      "The query executor has not been initialised");

        protected ICommandExecutor CommandExecutor => _commandExecutor
                                                      ?? throw new NullReferenceException(
                                                          "The command executor has not been initialised");

        protected ILogger Logger => _logger
                                    ?? throw new NullReferenceException(
                                        "The logger has not been initialised");

        protected IDbConnection Db => _dbConnection
                                                ?? throw new NullReferenceException(
                                                    "The connection has not been initialised");

        protected IServiceProvider Sp => _serviceProvider ??
                                         throw new NullReferenceException(
                                             "The service provider has not been initialised");

        public void Initialise(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dbConnection = serviceProvider.GetRequiredService<IDbConnection>();
            _queryExecutor = serviceProvider.GetRequiredService<IQueryExecutor>();
            _commandExecutor = serviceProvider.GetRequiredService<ICommandExecutor>();
            _logger = serviceProvider.GetRequiredService<ILogger<SqlExecutor>>();
        }

        public virtual Task<T> QueryFirst<T>(string sql, object? parameters = null)
        {
            var result = Db.QueryFirst<T>(sql, parameters);
            return Task.FromResult(result);
        }

        public virtual Task<IList<TReturn>> QueryList<TFirst, TSecond, TReturn>(
            string sql,
            Func<TFirst, TSecond, TReturn> map,
            object? parameters = null)
        {
            return Task.FromResult<IList<TReturn>>(Db.Query(sql, map, parameters).ToList());
        }

        public virtual Task<IList<TReturn>> QueryList<TFirst, TSecond, TThird, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TReturn> map,
            object? parameters = null)
        {
            return Task.FromResult<IList<TReturn>>(Db
                .Query(sql, map, parameters)
                .ToList());
        }

        public virtual Task<IList<TReturn>> QueryList<TFirst, TSecond, TThird, TFourth, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map,
            object? parameters = null)
        {
            return Task.FromResult<IList<TReturn>>(Db
                .Query(sql, map, parameters)
                .ToList());
        }

        public virtual Task<IList<TReturn>> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map,
            object? parameters = null)
        {
            return Task.FromResult<IList<TReturn>>(Db
                .Query(sql, map, parameters)
                .ToList());
        }

        public virtual Task<IList<TReturn>> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
            object? parameters = null)
        {
            return Task.FromResult<IList<TReturn>>(Db
                .Query(sql, map, parameters)
                .ToList());
        }

        public virtual Task<IList<TReturn>> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh,
            TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
            object? parameters = null)
        {
            return Task.FromResult<IList<TReturn>>(Db
                .Query(sql, map, parameters)
                .ToList());
        }

        public virtual Task<IList<T>> QueryList<T>(string sql, object? parameters = null)
        {
            return Task.FromResult<IList<T>>(Db
                .Query<T>(sql, parameters)
                .ToList());
        }

        public virtual int Execute(string sql, object? parameters = null)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException("Please specify a value for the sql attribute.");
            }

            return Db.Execute(sql, parameters);
        }
    }
}