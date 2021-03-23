using System;
using System.Data;

namespace DapperDoodle.Interfaces
{
    public interface IBaseSqlExecutorOptions
    {
        IServiceProvider ServiceProvider { get; set; }
        IDbConnection Connection { get; set; }
        DBMS Dbms { get; set; }
    }

    public class BaseSqlExecutorOptions : IBaseSqlExecutorOptions
    {
        public IServiceProvider ServiceProvider { get; set; }
        public IDbConnection Connection { get; set; }
        public DBMS Dbms { get; set; }
    }
}