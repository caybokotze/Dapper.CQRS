using System.Threading.Tasks;

namespace Dapper.CQRS;

/// <summary>
/// Exposes the overloads which execute synchronous and asynchronous queries
/// </summary>
public class QueryExecutor : IQueryExecutor
{
    public void Execute(Query query)
    {
        query.QueryExecutor = this;
        
        if (BaseConnection.ValidateAmbientTransaction)
        {
            query.ValidateTransactionScope();
        }
        
        query.Execute();
    }

    public T Execute<T>(Query<T> query)
    {
        query.QueryExecutor = this;

        if (BaseConnection.ValidateAmbientTransaction)
        {
            query.ValidateTransactionScope();
        }
        
        return query.Execute();
    }

    public async Task ExecuteAsync(QueryAsync query)
    {
        query.QueryExecutor = this;
        
        if (BaseConnection.ValidateAmbientTransaction)
        {
            query.ValidateTransactionScope();
        }
        
        await query.ExecuteAsync();
    }
        
    public async Task<T> ExecuteAsync<T>(QueryAsync<T> query)
    {
        query.QueryExecutor = this;
        
        if (BaseConnection.ValidateAmbientTransaction)
        {
            query.ValidateTransactionScope();
        }
        
        return await query.ExecuteAsync();
    }
}