using System;
using System.Transactions;

namespace Dapper.CQRS;

internal class ConnectionConfiguration
{
    # region Static properties
    internal static IDbConnectionFactory? ConnectionFactory { get; set; }
    internal static IServiceProvider? ServiceProvider { get; set; }
    internal static string DefaultSplitOn { get; set; } = "Id";
    internal static int DefaultTimeout { get; set; } = 30;
    internal static IsolationLevel DefaultIsolationLevel { get; set; } = IsolationLevel.ReadCommitted;
    internal static TransactionScopeOption DefaultTransactionScopeOption { get; set; } = TransactionScopeOption.Required;
    internal static bool ValidateAmbientTransaction { get; set; }
    internal static bool CreateTransaction { get; set; }
    internal static bool AutoRollback { get; set; }
    
    
    #endregion
    
    internal int ScopedTimeout { get; set; } = DefaultTimeout;
    internal string ScopedSplitOn { get; set; } = DefaultSplitOn;
    internal string? ScopedConnectionName { get; set; }
}