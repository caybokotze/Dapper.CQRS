using System.Transactions;
using Dapper.CQRS.Tests.TestModels;
using GenericSqlBuilder;

namespace Dapper.CQRS.Tests.Commands
{
    public class InsertUser : Command<int>
    {
        public User User { get; }

        public InsertUser(User user)
        {
            User = user;
        }
        
        public override void Execute()
        {
            using var scope = new TransactionScope();
            var sql = new SqlBuilder()
                .Insert<User>("users", i =>
                {
                    i.RemoveProperty(nameof(User.UserType));
                    i.RemoveProperty(nameof(User.UserDetails));
                })
                .Values()
                .Build();
            
            Execute(sql, User);
            Result = CommandExecutor.Execute(new InsertUserType(User.UserType));
            scope.Complete();
        }
    }
}