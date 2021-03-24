using System.Collections.Generic;
using DapperDoodle.Interfaces;

namespace DapperDoodle
{
    public abstract class Query<T> : Query
    {
        public T Result { get; protected set; }
    }
    
    public abstract class Query : BaseSqlExecutor
    {
        public abstract void Execute();

        public List<T> BuildSelect<T>(object parameters)
        {
            return QueryList<T>(this.BuildSelectStatement<T>(), parameters);
        }
        
        public List<T> BuildSelect<T>(object parameters, string clause)
        {
            return QueryList<T>(this.BuildSelectStatement<T>(clause), parameters);
        }

        public List<T> BuildSelect<T>(object parameters, string clause, string table)
        {
            return QueryList<T>(this.BuildSelectStatement<T>(clause, table), parameters);
        }
        
        public List<T> BuildSelect<T>(object parameters, string clause, string table, Case @case)
        {
            return QueryList<T>(this.BuildSelectStatement<T>(clause, table, @case), parameters);
        }

        public List<T> BuildSelect<T>(object parameters, string clause, string table, Case @case, object ignoreParameters)
        {
            return QueryList<T>(this.BuildSelectStatement<T>(clause, table, @case, ignoreParameters), parameters: parameters);
        }
    }
}