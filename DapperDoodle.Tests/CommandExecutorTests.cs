using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using static PeanutButter.RandomGenerators.RandomValueGen;

namespace DapperDoodle.Tests
{
    [TestFixture]
    public class CommandExecutorTests
    {
        [TestFixture]
        public class Registrations : ResolveExecutors
        {
            [Test]
            public void AssertThatIApplicationBuilderRegistersCommandExecutor()
            {
                var actual = GetRandomInt();
                if (CommandExecutor == null) return;
                
                var expected = CommandExecutor
                    .Execute(new CommandInheritor(actual));
            
                Assert.AreEqual(actual, expected);
            }

            public void AssertThatCommandExecutorIsAccessibleFromWithinCommand()
            {
                // var serviceProvider =
            }

            public class CommandInheritor : Command<int>
            {
                private readonly int _expectedReturnValue;

                public CommandInheritor(int expectedReturnValue)
                {
                    _expectedReturnValue = expectedReturnValue;
                }
                public override void Execute()
                {
                    Result = QueryFirst<int>($"Select {_expectedReturnValue};");
                }
            }
        }
    }
}