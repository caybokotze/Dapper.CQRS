using System.Transactions;
using Dapper.CQRS.Tests.Queries;
using Dapper.CQRS.Tests.TestModels;
using GenericSqlBuilder;

namespace Dapper.CQRS.Tests.Commands
{
    public class InsertUser : Command
    {
        public User User { get; }

        public InsertUser(User user)
        {
            User = user;
        }
        
        public override void Execute()
        {
            using var scope = new TransactionScope();
            var insertUser = new SqlBuilder()
                .Insert<User>("users", i =>
                {
                    i.RemoveMultipleProperties(User.NotMapped());
                    i.UsePropertyCase(Casing.SnakeCase);
                })
                .Values()
                .Build();

            var existingUser = QueryExecutor.Execute(new FetchUser(User.Id));

            if (existingUser is null)
            {
                Execute(insertUser, User);
            }
            
            scope.Complete();
        }
    }
}