﻿using System;
using DapperDiddle.Commands;
using DapperDiddle.Enums;
using DapperDiddle.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace DapperDiddle
{
    public static class ConfigurationHelpers
    {
        public static void ConfigureDapperDiddle(
            this IServiceCollection services, 
            string connectionString,
            DBMS dbms)
        {
            if (services is null)
                throw new NullReferenceException("The service collection Specified is invalid");
            
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
                    throw new ArgumentException("Invalid Database selection.");
                }
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
            services.AddScoped<IBaseSqlExecutorOptions>(provider =>
                new BaseSqlExecutorOptions()
                {
                    Connection = new MySqlConnection(connectionString),
                    Dbms = dbms
                });

            services.AddScoped<ICommandExecutor, CommandExecutor>();
        }

        private static void ConfigureForSqLite(
            this IServiceCollection services,
            string connectionString,
            DBMS dbms)
        {
            if (connectionString is null) connectionString = "Data Source=app.db";
            
            services.AddScoped<IBaseSqlExecutorOptions>(provider => new BaseSqlExecutorOptions()
            {
                Connection = new SqliteConnection(connectionString),
                Dbms = dbms
            });
        }
    }
}