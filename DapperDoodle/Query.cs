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

        public List<T> BuildSelect<T>(object parameters = null)
        {
            return QueryList<T>(this.BuildSelectStatement<T>(), parameters);
        }
        
        public List<T> BuildSelect<T>(Case @case)
        {
            return QueryList<T>(this.BuildSelectStatement<T>(@case));
        }

        public List<T> BuildSelect<T>(string clause, object parameters = null)
        {
            return QueryList<T>(this.BuildSelectStatement<T>(null, clause), parameters);
        }
        
        public List<T> BuildSelect<T>(string table, string clause, object parameters = null)
        {
            return QueryList<T>(this.BuildSelectStatement<T>(table, clause), parameters: parameters);
        }
        
        public List<T> BuildSelect<T>(string table, string clause, Case @case, object parameters = null, object ignoreParameters = null)
        {
            return QueryList<T>(this.BuildSelectStatement<T>(table, @case, clause, ignoreParameters), parameters: parameters);
        }
    }
}