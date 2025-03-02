using System.Data;

namespace Dapper.CQRS;

public interface IDbConnectionFactory
{
    /// <summary>
    /// Returns an instance of IDbConnection
    /// </summary>
    /// <returns></returns>
    IDbConnection Create();
    
    /// <summary>
    /// To make use several different IDbConnection instance types.
    /// However, this can not be configured with the Func`IDbConnection option. Please implement your own Factory using this interface.
    /// </summary>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    IDbConnection Create(string connectionName);
}