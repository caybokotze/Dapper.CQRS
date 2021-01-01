using System;
using System.Collections.Generic;
using DapperDoodle.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DapperDoodle
{
    public class QueryExecutor : IQueryExecutor
    {
        public IBaseSqlExecutorOptions Options { get; }

        public QueryExecutor(IServiceProvider services)
        {
            Options = services.GetService<IBaseSqlExecutorOptions>();
        }
        
        public void Execute(Query query)
        {
            query.InitialiseDependencies(Options);
            ExecuteWithNoResult(query);
        }

        private void ExecuteWithNoResult(Query query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            query.QueryExecutor = this;
            query.Execute();
        }

        public T Execute<T>(Query<T> query)
        {
            query.InitialiseDependencies(Options);
            ExecuteWithNoResult(query);
            return query.Result;
        }

        public void Execute(IEnumerable<Query> queries)
        {
            /* Perhaps there should be an option to return a list of query results
                If one has a list of queries, the following options are equivalent
                
                Execute(listOfQueries);
                for (q in listOfQueries)
                    results.append(q.Result)
                

                for (q in listOfQueries)
                    results.append(Execute(q))
                

                results = Execute(listOfQueries)


                the second option is more concise than the first, so it removes the desire to be able to execute multiple queries.
                Allowing for the third case would be the most concise and probably ideal for this use-case?
            */
            if (queries == null)
            {
                throw new ArgumentNullException(nameof (queries));
            }

            foreach (Query query in queries)
            {
                Execute(query);
            }
        }
    }
}