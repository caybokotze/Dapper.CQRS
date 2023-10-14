using Dapper.CQRS.Tests.Queries;
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
        
        public override int Execute()
        {
            var insertSql = new SqlBuilder()
                .Insert<User>("users", i =>
                {
                    i.RemoveProperty(nameof(User.Id));
                    i.RemoveMultipleProperties(User.NotMapped());
                    i.UsePropertyCase(Casing.SnakeCase);
                })
                .Values()
                .AppendStatement()
                .Select()
                .LastInserted(Version.MySql);

            var result = QueryExecutor.Execute(new FetchUser(User.Id));

            if (result.Success)
            {
                return 1;
            }

            if (result.Failure)
            {
                var insertedUserResult = QueryFirst<int>(insertSql, User);
                
                if (insertedUserResult > 0)
                {
                }
            }

            return 1;
        }
    }
}