using System.Threading.Tasks;
using Dapper.CQRS.Tests.TestModels;
using GenericSqlBuilder;

namespace Dapper.CQRS.Tests.Queries;

public class FetchUserDetailsByUserId : QueryAsync<UserDetail?>
{
    private readonly int _userId;

    public FetchUserDetailsByUserId(int userId)
    {
        _userId = userId;
    }
    
    public override async Task<UserDetail?> ExecuteAsync()
    {
        return await QueryFirstOrDefaultAsync<UserDetail>(
            new SqlBuilder()
            .Select<UserDetail>(s =>
            {
                s.UsePropertyCase(Casing.SnakeCase);
            }).From("user_details")
            .Where("user_id = @UserId").Build(), new { UserId = _userId });
    }
}