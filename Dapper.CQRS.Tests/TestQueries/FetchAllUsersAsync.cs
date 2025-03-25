using System.Linq;
using System.Threading.Tasks;
using Dapper.CQRS.Tests.TestModels;
using GenericSqlBuilder;

namespace Dapper.CQRS.Tests.TestQueries;

public class FetchAllUsersAsync : QueryAsync<User[]>
{
    public override async Task<User[]> ExecuteAsync()
    {
        return (await QueryListAsync<User>(
                new SqlBuilder()
                    .Select<User>(s =>
                    {
                        s.UsePropertyCase(Casing.SnakeCase);
                        s.RemoveMultipleProperties(User.NotMapped());
                    })
                    .From("users")
                    .Build()))
            .ToArray();
    }
}