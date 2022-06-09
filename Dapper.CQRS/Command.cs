using System.Transactions;

namespace Dapper.CQRS
{
    public interface ICommand
    {
        void Execute();
    }
    
    public abstract class Command<T> : Command
    {
        public T Result { get; set; }
    }
    
    public abstract class Command : BaseSqlExecutor, ICommand
    {
        public Command()
        {
            
        }

        public abstract void Execute();
    }
}