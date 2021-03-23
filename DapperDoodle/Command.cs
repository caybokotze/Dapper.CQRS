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
        protected Command()
        {
            QueryExecutor = new QueryExecutor(Provider);
            CommandExecutor = new CommandExecutor(Provider);
        }
        public abstract void Execute();
        
        public IQueryExecutor QueryExecutor { get; set; }
        public ICommandExecutor CommandExecutor { get; set; }

        /// <summary>
        /// Returns the ID of the Inserted record after inserting the record.
        /// </summary>
        /// <param name="parameters">Pass in the arguments or type as an argument that needs to be appended to the sql statement</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int BuildInsert<T>(object parameters)
        {
            return QueryFirst<int>(this.BuildInsertStatement<T>(), parameters: parameters);
        }

        public int BuildInsert<T>(object parameters, string table)
        {
            return QueryFirst<int>(this.BuildInsertStatement<T>(table: table), parameters: parameters);
        }
        
        public int BuildInsert<T>(object parameters, string table, Case @case)
        {
            return QueryFirst<int>(this.BuildInsertStatement<T>(table: table, @case: @case), parameters: parameters);
        }
        
        public int BuildInsert<T>(object parameters, string table, Case @case, object removeParameters)
        {
            return QueryFirst<int>(this.BuildInsertStatement<T>(table: table, @case: @case, removeParameters: removeParameters), parameters: parameters);
        }
        
        public int BuildUpdate<T>(object parameters, string clause)
        {
            return QueryFirst<int>(this.BuildUpdateStatement<T>(table: null, clause: clause), parameters: parameters);
        }
        
        public int BuildUpdate<T>(object parameters, string table, string clause)
        {
            return QueryFirst<int>(this.BuildUpdateStatement<T>(table: table, clause: clause), parameters: parameters);
        }

        public int BuildUpdate<T>(object parameters)
        {
            return QueryFirst<int>(this.BuildUpdateStatement<T>(), parameters: parameters);
        }

        public int BuildDelete<T>(object parameters)
        {
            return QueryFirst<int>(this.BuildDeleteStatement<T>(), parameters: parameters);
        }

        /// <summary>
        /// This will automatically append the last inserted id for the record inserted.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="InvalidSqlStatementException"></exception>
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