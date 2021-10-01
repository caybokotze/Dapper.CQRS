using System;
using System.Configuration;
using System.Data;
using System.Runtime.Serialization;
using Dapper.CQRS.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using NExpect;
using NSubstitute;
using NUnit.Framework;
using static NExpect.Expectations;
using static NSubstitute.Substitute;
using static PeanutButter.RandomGenerators.RandomValueGen;

namespace Dapper.CQRS.Tests
{
    [TestFixture]
    public class BaseSqlExecutorTests
    {
        [TestFixture]
        public class BaseSqlExecutorOptionsTests
        {
            [Test]
            public void ShouldContainAllRequiredProperties()
            {
                
            }
        }

        [TestFixture]
        public class Injection : TestBase
        {
            [Test]
            public void ShouldResolveExpectedConnectionString()
            {
                var serviceProvider = For<IServiceProvider>();
                var baseSqlExecutor = For<BaseSqlExecutor>();
                var dbConnection = new MySqlConnection(GenerateValidMySqlConnectionString());

                baseSqlExecutor.InitialiseDependencies(new CqrsSqlExecutorOptions
                {
                    ServiceProvider = serviceProvider,
                    Connection = dbConnection,
                    Dbms = DBMS.MySQL
                });

                Expect(baseSqlExecutor.Raw().ConnectionString)
                    .To.Contain("server=");
            }

            [Test]
            public void ShouldBeInjectedWithInstanceOfIServiceProviderAndConfigureExecutors()
            {
                var serviceProvider = For<IServiceProvider>();
                var baseSqlExecutor = For<BaseSqlExecutor>();
                var dbConnection = new MySqlConnection(GenerateValidMySqlConnectionString());

                baseSqlExecutor.InitialiseDependencies(new CqrsSqlExecutorOptions
                {
                    ServiceProvider = serviceProvider,
                    Connection = dbConnection,
                    Dbms = DBMS.MySQL
                });

                Expect(baseSqlExecutor.CommandExecutor).To.Not.Be.Null();
                Expect(baseSqlExecutor.QueryExecutor).To.Not.Be.Null();
            }
            
            [Test]
            public void ShouldBeInjectedWithInstanceOfCommandExecutor()
            {
                
            }

            [Test]
            public void ShouldBeInjectedWithInstanceOfQueryExecutor()
            {
                
            }

            [Test]
            public void ShouldBeInjectedWithInstanceOfConnectionString()
            {
                
            }

            [Test]
            public void ShouldBeInjectedWithInstanceOfDbmsType()
            {
                
            }
        }
    }
}