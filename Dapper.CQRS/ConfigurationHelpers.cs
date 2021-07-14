using System;
using Dapper.CQRS.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace Dapper.CQRS
{
    public static class ConfigurationHelpers
    {
        public static void ConfigureDefaults(
            this IServiceCollection services, 
            string connectionString,
            DBMS dbms)
        {
            if (services is null)
            {
                throw new NullReferenceException("The service collection Specified is invalid");
            }
            
            ConfigureCqrsInterfaces(services);
            
            switch (dbms)
            {
                case DBMS.MySQL:
                {
                    ConfigureForMySql(services, connectionString, dbms);
                    break;
                }
                case DBMS.SQLite:
                {
                    ConfigureForSqLite(services, connectionString, dbms);
                    break;
                }
                // todo: Add the case for MSSQL.
                default:
                {
                    throw new ArgumentException("The database you have selected is not yet supported.");
                }
            }
        }

        private static void ConfigureCqrsInterfaces(this IServiceCollection services)
        {
            services.AddScoped<ICommandExecutor, CommandExecutor>();
            services.AddScoped<IQueryExecutor, QueryExecutor>();
        }

        private static void ConfigureForMySql(
            this IServiceCollection services,
            string connectionString,
            DBMS dbms)
        {
            if (connectionString is null)
            {
                throw new ArgumentNullException("Please specify a valid connection string for MySql to use.");
            }
            
            services.AddScoped<IBaseSqlExecutorOptions>(provider =>
                new BaseSqlExecutorOptions()
                {
                    ServiceProvider = provider,
                    Connection = new MySqlConnection(connectionString),
                    Dbms = dbms
                });
        }

        private static void ConfigureForSqLite(
            this IServiceCollection services,
            string connectionString,
            DBMS dbms)
        {
            if (connectionString is null) connectionString = "Data Source=app.db";
            
            services.AddScoped<IBaseSqlExecutorOptions>(provider => 
                new BaseSqlExecutorOptions()
            {
                ServiceProvider = provider,
                Connection = new SqliteConnection(connectionString),
                Dbms = dbms
            });
        }
    }
}