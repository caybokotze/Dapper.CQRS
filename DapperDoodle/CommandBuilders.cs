using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using DapperDoodle.Exceptions;

namespace DapperDoodle
{
    public static class CommandBuilders
    {
        public static string BuildInsertStatement<T>(this Command command)
        {
            return BuildInsertStatement<T>(command, null, Case.SnakeCase, null);
        }

        public static string BuildInsertStatement<T>(this Command command, string table)
        {
            return BuildInsertStatement<T>(command, table, Case.SnakeCase, null);
        }
        
        public static string BuildInsertStatement<T>(this Command command, string table, Case @case)
        {
            return BuildInsertStatement<T>(command, table, @case, null);
        }
        
        /// <summary>
        /// Returns a string of the Insert Statement that will be inserted into the database.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="table"></param>
        /// <param name="case"></param>
        /// <param name="removeParameters"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string BuildInsertStatement<T>(this Command command, string table, Case @case, object removeParameters)
        {
            var dt = typeof(T).DataTableForType();

            table ??= typeof(T).Name.Pluralize().ConvertCase(@case);

            var sqlStatement = new StringBuilder();

            dt = RemovePropertiesFromDataTable(dt, removeParameters);

            switch (command.Dbms)
            {
                case DBMS.MySQL:
                    sqlStatement.Append($"INSERT INTO `{table}` (");
                    break;
                
                case DBMS.SQLite:
                    sqlStatement.Append($"INSERT INTO {table} (");
                    break;
                
                case DBMS.MSSQL:
                    sqlStatement.Append($"INSERT INTO [{table}] (");
                    break;
                default:
                    throw new InvalidDatabaseTypeException();
            }

            foreach (DataColumn column in dt.Columns)
            {
                sqlStatement.Append($"{column.ColumnName.ConvertCase(@case)}, ");
            }

            sqlStatement.Remove(sqlStatement.Length -2, 2);
            sqlStatement.Append(") VALUES (");

            foreach (DataColumn column in dt.Columns)
            {
                sqlStatement.Append($"@{column.ColumnName}, ");
            }

            sqlStatement.Remove(sqlStatement.Length - 2, 2);
            sqlStatement.Append("); ");

            command.AppendReturnId(sqlStatement);
            
            return sqlStatement.ToString();
        }
        
        public static string BuildUpdateStatement<T>(this Command command)
        {
            return BuildUpdateStatement<T>(command, null, null, Case.SnakeCase, null);
        }
        
        public static string BuildUpdateStatement<T>(this Command command, string clause)
        {
            return BuildUpdateStatement<T>(command, clause, null, Case.SnakeCase, null);
        }
        
        public static string BuildUpdateStatement<T>(this Command command, string clause, string table)
        {
            return BuildUpdateStatement<T>(command, clause, table, Case.SnakeCase, null);
        }

        public static string BuildUpdateStatement<T>(this Command command, string clause, string table, Case @case)
        {
            return BuildUpdateStatement<T>(command, clause, table, @case, null);
        }

        /// <summary>
        /// Returns a SQL UPDATE statement with the override for a where clause and an override for the case and table name.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="case"></param>
        /// <param name="clause"></param>
        /// <param name="table"></param>
        /// <param name="removeParameters"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidDatabaseTypeException"></exception>
        public static string BuildUpdateStatement<T>(this Command command, string clause, string table, Case @case, object removeParameters)
        {
            var dt = typeof(T).DataTableForType();

            table ??= typeof(T).Name.Pluralize().ConvertCase(@case);
            
            clause ??= "WHERE id = @Id;";

            var sqlStatement = new StringBuilder();
            
            dt = RemovePropertiesFromDataTable(dt, removeParameters);

            switch (command.Dbms)
            {
                case DBMS.SQLite:
                    sqlStatement.Append($"UPDATE {table} SET");
                    break;
                case DBMS.MySQL:
                    sqlStatement.Append($"UPDATE `{table}` SET");
                    break;
                case DBMS.MSSQL:
                    sqlStatement.Append($"UPDATE [{table}] SET");
                    break;
                default:
                    throw new InvalidDatabaseTypeException();
            }

            sqlStatement.Append(" ");
            
            foreach (DataColumn column in dt.Columns)
            {
                sqlStatement.Append($"{column.ColumnName.ConvertCase(@case)} = @{column.ColumnName}, ");
            }

            sqlStatement.Remove(sqlStatement.Length - 2, 2);

            sqlStatement.Append(" ");

            sqlStatement.Append(clause);

            sqlStatement.Append(" SELECT 0;");
            
            return sqlStatement.ToString();
        }


        public static string BuildDeleteStatement<T>(this Command command)
        {
            return BuildDeleteStatement<T>(command, null, null, Case.SnakeCase, null);
        }
        
        public static string BuildDeleteStatement<T>(this Command command, string clause)
        {
            return BuildDeleteStatement<T>(command, clause, null, Case.SnakeCase, null);
        }
        
        public static string BuildDeleteStatement<T>(this Command command, string clause, string table)
        {
            return BuildDeleteStatement<T>(command, clause, table, Case.SnakeCase, null);
        }
        
        public static string BuildDeleteStatement<T>(this Command command, string clause, string table, Case @case)
        {
            return BuildDeleteStatement<T>(command, clause, table, @case, null);
        }

        /// <summary>
        /// Returns a SQL DELETE statement.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="clause"></param>
        /// <param name="table"></param>
        /// <param name="removeParameters"></param>
        /// <param name="case"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidDatabaseTypeException"></exception>
        public static string BuildDeleteStatement<T>(this Command command, string clause, string table, Case @case, object removeParameters)
        {
            var dt = typeof(T).DataTableForType();
            
            table ??= typeof(T).Name.Pluralize().ConvertCase(@case);

            clause ??= "WHERE id = @Id";
            
            var sqlStatement = new StringBuilder();
            
            dt = RemovePropertiesFromDataTable(dt, removeParameters);

            switch (command.Dbms)
            {
                case DBMS.SQLite:
                    sqlStatement.Append($"DELETE FROM {table}");
                    break;
                case DBMS.MySQL:
                    sqlStatement.Append($"DELETE FROM `{table}`");
                    break;
                case DBMS.MSSQL:
                    sqlStatement.Append($"DELETE FROM [{table}]");
                    break;
                default:
                    throw new InvalidDatabaseTypeException();
            }

            sqlStatement.Append(" ");
            sqlStatement.Append(clause);

            return sqlStatement.ToString();
        }

        private static DataTable RemovePropertiesFromDataTable(DataTable dt, object properties)
        {
            if (properties == null)
            {
                return dt;
            }

            var type = properties.GetType();
            var props = new List<PropertyInfo>(type.GetProperties());

            foreach (var prop in props)
            {
                dt.Columns.Remove(prop.Name);
            }

            return dt;
        }
    }
}