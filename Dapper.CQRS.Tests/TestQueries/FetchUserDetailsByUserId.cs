using System.Threading.Tasks;
using Dapper.CQRS.Tests.TestModels;
using GenericSqlBuilder;

namespace Dapper.CQRS.Tests.TestQueries;

public class FetchUserDetailsByUserId : QueryAsync<UserDetails?>
{
    private readonly int _userId;

    public FetchUserDetailsByUserId(int userId)
    {
        _userId = userId;
    }
    
    public override async Task<UserDetails?> ExecuteAsync()
    {
        return await QueryFirstOrDefaultAsync<UserDetails>(
            new SqlBuilder()
            .Select<UserDetails>(s =>
            {
                s.UsePropertyCase(Casing.SnakeCase);
            }).From("user_details")
            .Where("user_id = @UserId").Build(), new { UserId = _userId });
    }
}