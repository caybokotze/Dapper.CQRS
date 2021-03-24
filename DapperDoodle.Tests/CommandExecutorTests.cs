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

            [Test]
            public void AssertThatCommandExecutorIsAccessibleFromWithinCommand()
            {
                // Assert
                var number1 = GetRandomInt(100, 500);
                var number2 = GetRandomInt(100, 500);
                var expected = number1 + number2;
                // Act
                var result = CommandExecutor
                    .Execute(new NestedCommandExecutor(number1, number2));
                // Assert
                Assert.That(result.Equals(expected));
            }

            class NestedCommandExecutor : Command<int>
            {
                private readonly int _number1;
                private readonly int _number2;

                public NestedCommandExecutor(int number1, int number2)
                {
                    _number1 = number1;
                    _number2 = number2;
                }
                
                public override void Execute()
                {
                    var firstCommandResult = CommandExecutor
                        .Execute(new FirstCommand(_number1));
                    
                    var secondCommandResult = CommandExecutor
                        .Execute(new FirstCommand(_number2));

                    Result = firstCommandResult + secondCommandResult;
                }

                class FirstCommand : Command<int>
                {
                    private readonly int _number;

                    public FirstCommand(int number)
                    {
                        _number = number;
                    }
                    
                    public override void Execute()
                    {
                        Result = QueryFirst<int>($"SELECT {_number}");
                    }
                }

                class SecondCommand : Command<int>
                {
                    private readonly int _number;

                    public SecondCommand(int number)
                    {
                        _number = number;
                    }
                    
                    public override void Execute()
                    {
                        Result = QueryFirst<int>($"SELECT {_number}");
                    }
                }
            }

            class CommandInheritor : Command<int>
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