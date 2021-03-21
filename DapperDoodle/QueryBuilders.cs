﻿using System;
using System.Data;
using System.Text;
using DapperDoodle.Exceptions;

namespace DapperDoodle
{
    public static class QueryBuilders
    {
        public static string BuildSelectStatement<T>(this Query query)
        {
            return BuildSelectStatement<T>(query, null, Case.Lowercase);
        }
        
        public static string BuildSelectStatement<T>(this Query query, string table, string clause)
        {
            return BuildSelectStatement<T>(query, table, Case.Lowercase, clause);
        }
        
        public static string BuildSelectStatement<T>(this Query query, Case @case)
        {
            return BuildSelectStatement<T>(query, null, @case);
        }
        
        public static string BuildSelectStatement<T>(this Query query, string table, Case casing, string clause = null)
        {
            var dt = typeof(T).DataTableForType();

            if (table is null)
                table = typeof(T).Name.Pluralize().ConvertCase(casing);

            if (clause is null)
                clause = string.Empty;
            
            var sqlStatement = new StringBuilder();
            
            var variables = new StringBuilder();

            foreach (DataColumn column in dt.Columns)
            {
                variables.Append($"{column.ColumnName.ConvertCase(casing)}, ");
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

            sqlStatement.Append(" ");
            sqlStatement.Append(clause);
            sqlStatement.Append(";");

            return sqlStatement.ToString();
        }
    }
}