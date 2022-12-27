using System.Threading.Tasks;
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
        
        public override async Task<int> Execute()
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

            var existingUser = await QueryExecutor.Execute(new FetchUser(User.Id));

            if (existingUser is null)
            {
                return await QueryFirst<int>(insertSql);
            }

            return existingUser.Id;
        }
    }
}