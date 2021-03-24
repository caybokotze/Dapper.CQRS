using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using PluralizeService.Core;

namespace DapperDoodle
{
    public static class BuilderHelpers
    {
        public static DataTable RemovePropertiesFromDataTable(DataTable dt, object properties)
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
        
        public static DataTable DataTableForType(this Type type)
        {
            return (type?.GetProperties() ?? Array.Empty<PropertyInfo>())
                .Aggregate(
                    new DataTable(),
                    (acc, cur) =>
                    {
                        acc.Columns.Add(cur.Name,
                            Nullable.GetUnderlyingType(cur.PropertyType)
                            ?? cur.PropertyType);
                        return acc;
                    });
        }

        public static string Pluralize(this string value)
        {
            return PluralizationProvider.Pluralize(value);
        }

        public static string AppendReturnId(
            this BaseSqlExecutor executor, 
            StringBuilder builder)
        {
            switch (executor.Dbms)
            {
                case DBMS.SQLite:
                    builder.Append("SELECT last_insert_rowid();");
                    break;
                
                case DBMS.MySQL:
                    builder.Append("SELECT LAST_INSERT_ID();");
                    break;
                
                case DBMS.MSSQL:
                    builder.Append("SELECT SCOPE_IDENTITY();");
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return builder.ToString();
        }

        public static string AppendReturnId(
            this BaseSqlExecutor executor, 
            string originalString)
        {
            var builder = new StringBuilder(originalString);
            return AppendReturnId(executor, builder);
        }
    }
}