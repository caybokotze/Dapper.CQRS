using System.Data;
using System.Text;
using DapperDoodle.Exceptions;

namespace DapperDoodle
{
    public static class QueryBuilders
    {
        public static string BuildSelectStatement<T>(this Query query)
        {
            return BuildSelectStatement<T>(query, null, null, Case.SnakeCase, null);
        }
        
        public static string BuildSelectStatement<T>(this Query query, string clause)
        {
            return BuildSelectStatement<T>(query, clause, null, Case.SnakeCase, null);
        }
        
        public static string BuildSelectStatement<T>(this Query query, string clause, string table)
        {
            return BuildSelectStatement<T>(query, clause, table, Case.SnakeCase, null);
        }
        
        public static string BuildSelectStatement<T>(this Query query, string clause, string table, Case @case)
        {
            return BuildSelectStatement<T>(query, clause, table, @case, null);
        }
        
        public static string BuildSelectStatement<T>(this Query query, string clause, string table, Case @case, object removeParameters)
        {
            var dt = typeof(T).DataTableForType();

            table ??= typeof(T).Name.Pluralize().ConvertCase(@case);
            
            var sqlStatement = new StringBuilder();
            
            var variables = new StringBuilder();

            dt = BuilderHelpers.RemovePropertiesFromDataTable(dt, removeParameters);

            foreach (DataColumn column in dt.Columns)
            {
                variables.Append($"{column.ColumnName.ConvertCase(@case)}, ");
            }

            variables.Remove(variables.Length - 2, 2);

            switch (query.Dbms)
            {
                case DBMS.MySQL:
                    sqlStatement.Append($"SELECT {variables} FROM {table}");
                    break;
                case DBMS.SQLite:
                    sqlStatement.Append($"SELECT {variables} FROM `{table}`");
                    break;
                case DBMS.MSSQL:
                    sqlStatement.Append($"SELECT {variables} FROM [{table}]");
                    break;
                default:
                    throw new InvalidDatabaseTypeException();
            }

            if (clause != null)
            {
                sqlStatement.Append(" ");
                sqlStatement.Append(clause);
            }
            
            sqlStatement.Append(";");

            return sqlStatement.ToString();
        }
    }
}