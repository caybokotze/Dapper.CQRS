using System;

namespace Dapper.CQRS;

public struct QueryDetails
{
    public Type QueryType { get; }
    public string QueryName { get; set; }

    public QueryDetails(Type queryType, string queryName)
    {
        QueryType = queryType;
        QueryName = queryName;
    }
}

public interface IQueryExecutorEmitter
{
    public EventHandler<QueryDetails> Handle(Type queryType);
}