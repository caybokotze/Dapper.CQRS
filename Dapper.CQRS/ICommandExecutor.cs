using System.Collections.Generic;

namespace Dapper.CQRS
{
    public interface ICommandExecutor
    {
        void Execute(Command command);
        T Execute<T>(Command<T> command);
    }
}