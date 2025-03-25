using System.Threading.Tasks;
using Dapper.CQRS.Tests.TestModels;
using GenericSqlBuilder;

namespace Dapper.CQRS.Tests.TestQueries;

public class FetchUserByIdAsync : QueryAsync<User?>
{
    private readonly int _userId;

    public FetchUserByIdAsync(int userId)
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