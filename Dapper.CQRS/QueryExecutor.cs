using System;
using System.Threading.Tasks;

namespace Dapper.CQRS;

/// <summary>
/// Exposes the overloads which execute synchronous and asynchronous queries
/// </summary>
public interface IQueryExecutor
{
    /// <summary>
    /// Executes a synchronous query without a result.
    /// </summary>
    /// <param name="query"></param>
    void Execute(Query query);
    
    /// <summary>
    /// Executes a synchronous query and returns the result
    /// </summary>
    /// <param name="query"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T Execute<T>(Query<T> query);

    /// <summary>
    /// Executes a asynchronous query without a result
    /// </summary>
    /// <param name="queryAsync"></param>
    /// <returns></returns>
    Task ExecuteAsync(QueryAsync queryAsync);
    
    /// <summary>
    /// Executes a asynchronous query and returns a result
    /// </summary>
    /// <param name="query"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T> ExecuteAsync<T>(QueryAsync<T> query);
}

/// <summary>
/// Exposes the overloads which execute synchronous and asynchronous queries
/// </summary>
public class QueryExecutor : IQueryExecutor
{
    private readonly IServiceProvider _serviceProvider;

    public QueryExecutor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public void Execute(Query query)
    {
        query.QueryExecutor = this;
        query.InitialiseExecutor(_serviceProvider);
        query.Execute();
    }

    public T Execute<T>(Query<T> query)
    {
        query.QueryExecutor = this;
        query.InitialiseExecutor(_serviceProvider);
        return query.Execute();
    }

    public async Task ExecuteAsync(QueryAsync query)
    {
        query.QueryExecutor = this;
        query.InitialiseExecutor(_serviceProvider);
        await query.ExecuteAsync();
    }
        
    public async Task<T> ExecuteAsync<T>(QueryAsync<T> query)
    {
        query.QueryExecutor = this;
        query.InitialiseExecutor(_serviceProvider);
        return await query.ExecuteAsync();
    }
}