#nullable enable
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dapper.CQRS
{
    public interface IBaseSqlExecutor
    {
        void Initialise(IServiceProvider serviceProvider);

        T QueryFirst<T>(string sql, object? parameters = null);

        IList<TReturn> QueryList<TFirst, TSecond, TReturn>(
            string sql,
            Func<TFirst, TSecond, TReturn> map,
            object? parameters = null);

        IList<TReturn> QueryList<TFirst, TSecond, TThird, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TReturn> map,
            object? parameters = null);

        IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map,
            object? parameters = null);

        IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map,
            object? parameters = null);

        IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
            object? parameters = null);

        IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
            object? parameters = null);

        IList<T> QueryList<T>(string sql, object? parameters = null);
        int Execute(string sql, object? parameters = null);
    }

    public class BaseSqlExecutor : IBaseSqlExecutor
    {
        private IQueryExecutor? _queryExecutor;
        private ICommandExecutor? _commandExecutor;

        protected IQueryExecutor QueryExecutor => _queryExecutor
                                                  ?? throw new NullReferenceException(
                                                      "The query executor has not been initialised");
        protected ICommandExecutor CommandExecutor => _commandExecutor
                                                      ?? throw new NullReferenceException(
                                                          "The command executor has not been initialised");

        protected ILogger? Logger { get; set; }

        private IDbConnection? _dbConnection;

        public void Initialise(IServiceProvider serviceProvider)
        {
            _dbConnection = serviceProvider.GetRequiredService<IDbConnection>();
            _queryExecutor = serviceProvider.GetRequiredService<IQueryExecutor>();
            _commandExecutor = serviceProvider.GetRequiredService<ICommandExecutor>();
            Logger = serviceProvider.GetRequiredService<ILogger<BaseSqlExecutor>>();
        }

        public T QueryFirst<T>(string sql, object? parameters = null)
        {
            return _dbConnection.QueryFirst<T>(sql, parameters);
        }

        public IList<TReturn> QueryList<TFirst, TSecond, TReturn>(
            string sql,
            Func<TFirst, TSecond, TReturn> map,
            object? parameters = null)
        {
            return _dbConnection
                .Query(sql, map, parameters)
                .ToList();
        }

        public IList<TReturn> QueryList<TFirst, TSecond, TThird, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TReturn> map,
            object? parameters = null)
        {
            return _dbConnection
                .Query(sql, map, parameters)
                .ToList();
        }
        
        public IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map,
            object? parameters = null)
        {
            return _dbConnection
                .Query(sql, map, parameters)
                .ToList();
        }
        
        public IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map,
            object? parameters = null)
        {
            return _dbConnection
                .Query(sql, map, parameters)
                .ToList();
        }
        
        public IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
            object? parameters = null)
        {
            return _dbConnection
                .Query(sql, map, parameters)
                .ToList();
        }
        
        public IList<TReturn> QueryList<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
            string sql,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
            object? parameters = null)
        {
            return _dbConnection
                .Query(sql, map, parameters)
                .ToList();
        }

        public virtual IList<T> QueryList<T>(string sql, object? parameters = null)
        {
            return _dbConnection
                .Query<T>(sql, parameters)
                .ToList();
        }

        public int Execute(string sql, object? parameters = null)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentException("Please specify a value for the sql attribute.");
            }
        
            return _dbConnection.Execute(sql, parameters);
        }
    }
}