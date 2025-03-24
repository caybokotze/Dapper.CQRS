using System.Threading.Tasks;
using Dapper.CQRS.Tests.TestModels;
using GenericSqlBuilder;

namespace Dapper.CQRS.Tests.Queries;

public class FetchUserById : QueryAsync<User?>
{
    private readonly int _userId;

    public FetchUserById(int userId)
    {
        _userId = userId;
    }

    public override async Task<User?> ExecuteAsync()
    {
        return await QueryFirstOrDefaultAsync<User>(
            new SqlBuilder()
                .Select<User>(s =>
                {
                    s.UsePropertyCase(Casing.SnakeCase);
                    s.RemoveMultipleProperties(User.NotMapped());
                })
                .From("users")
                .Where("id = @Id")
                .Build(), new {Id = _userId});
    }
}