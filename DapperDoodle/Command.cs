using DapperDoodle.Exceptions;

namespace DapperDoodle
{
    public interface ICommand
    {
        void Execute();
    }
    
    public abstract class Command<T> : Command
    {
        public T Result { get; set; }
    }
    
    public abstract class Command : BaseSqlExecutor, ICommand
    {
        public abstract void Execute();                                                                                                            
        
        # region BuildInsert
        public int BuildInsert<T>(object parameters)
        {
            return QueryFirst<int>(this.BuildInsertStatement<T>(), parameters: parameters);
        }

        public int BuildInsert<T>(object parameters, string table)
        {
            return QueryFirst<int>(this.BuildInsertStatement<T>(table), parameters: parameters);
        }
        
        public int BuildInsert<T>(object parameters, string table, Case @case)
        {
            return QueryFirst<int>(this.BuildInsertStatement<T>(table: table, @case: @case), parameters: parameters);
        }
        
        public int BuildInsert<T>(object parameters, string table, Case @case, object removeParameters)
        {
            return QueryFirst<int>(this.BuildInsertStatement<T>(table: table, @case: @case, removeParameters: removeParameters), parameters: parameters);
        }
        # endregion

        #region BuildUpdate
        public int BuildUpdate<T>(object parameters)
        {
            return QueryFirst<int>(this.BuildUpdateStatement<T>(), parameters: parameters);
        }
        
        public int BuildUpdate<T>(object parameters, string clause)
        {
            return QueryFirst<int>(this.BuildUpdateStatement<T>(clause), parameters: parameters);
        }
        
        public int BuildUpdate<T>(object parameters, string clause, string table)
        {
            return QueryFirst<int>(this.BuildUpdateStatement<T>(clause, table), parameters: parameters);
        }
        
        public int BuildUpdate<T>(object parameters, string clause, string table, Case @case)
        {
            return QueryFirst<int>(this.BuildUpdateStatement<T>(clause, table, @case), parameters: parameters);
        }
        
        public int BuildUpdate<T>(object parameters, string clause, string table, Case @case, object removeParameters)
        {
            return QueryFirst<int>(this.BuildUpdateStatement<T>(clause, table, @case, removeParameters), parameters: parameters);
        }
        #endregion
        
        # region BuildDelete
        public int BuildDelete<T>(object parameters)
        {
            return QueryFirst<int>(this.BuildDeleteStatement<T>(), parameters: parameters);
        }
        
        public int BuildDelete<T>(object parameters, string clause)
        {
            return QueryFirst<int>(this.BuildDeleteStatement<T>(clause), parameters: parameters);
        }
        
        public int BuildDelete<T>(object parameters, string clause, string table)
        {
            return QueryFirst<int>(this.BuildDeleteStatement<T>(clause, table), parameters: parameters);
        }
        
        public int BuildDelete<T>(object parameters, string clause, string table, Case @case)
        {
            return QueryFirst<int>(this.BuildDeleteStatement<T>(clause, table, @case), parameters: parameters);
        }
        
        public int BuildDelete<T>(object parameters, string clause, string table, Case @case, object removeParameters)
        {
            return QueryFirst<int>(this.BuildDeleteStatement<T>(clause, table, @case, removeParameters), parameters: parameters);
        }
        
        # endregion
        
        public int InsertAndReturnId(string sql, object parameters = null)
        {
            if (sql is null)
            {
                throw new InvalidSqlStatementException();
            }

            this.AppendReturnId(sql);
            
            return QueryFirst<int>(sql, parameters);
        }
    }
}