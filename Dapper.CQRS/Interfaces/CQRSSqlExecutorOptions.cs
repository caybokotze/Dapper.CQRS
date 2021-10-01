using System;
using System.Data;

namespace Dapper.CQRS.Interfaces
{
    public interface CQRSSqlExecutorOptions
    {
        IDbConnection Connection { get; set; }
        DBMS Dbms { get; set; }
        IServiceProvider ServiceProvider { get; set; }
    }

    public class CqrsSqlExecutorOptions : CQRSSqlExecutorOptions
    {
        public IDbConnection Connection { get; set; }
        public DBMS Dbms { get; set; }
        public IServiceProvider ServiceProvider { get; set; }
    }
}