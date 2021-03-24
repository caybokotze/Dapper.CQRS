using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DapperDoodle.Tests
{
    public class ResolveExecutors
    {
        public ICommandExecutor CommandExecutor { get; private set; }
        public IQueryExecutor QueryExecutor { get; private set; }

        [SetUp]
        public void Setup()
        {
            var serviceProvider = HostBuilder.CreateHostBuilder().Result;
            QueryExecutor = serviceProvider.GetService<IQueryExecutor>();
            CommandExecutor = serviceProvider.GetService<ICommandExecutor>();
        }
    }
}