using System.Linq;
using System.Threading.Tasks;
using Dapper.CQRS.Tests.TestModels;
using GenericSqlBuilder;

namespace Dapper.CQRS.Tests.Queries
{
    public class FetchUser : Query<User?>
    {
        public int Id { get; }

        public FetchUser(int id)
        {
            Id = id;
        }
        
        public override async Task<User?> Execute()
        {
            var users =  await QueryList<User>(
                new SqlBuilder()
                .Select<User>(s =>
                {
                    s.UsePropertyCase(Casing.SnakeCase);
                    s.RemoveMultipleProperties(User.NotMapped());
                })
                .From("users")
                .Build());

            return users.FirstOrDefault();
        }
    }
}