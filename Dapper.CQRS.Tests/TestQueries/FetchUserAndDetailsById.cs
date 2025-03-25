using System.Threading.Tasks;
using Dapper.CQRS.Tests.TestModels;
using GenericSqlBuilder;

namespace Dapper.CQRS.Tests.TestQueries;

public class FetchUserAndDetailsById : QueryAsync<User?>
{
    private readonly int _userId;

    public FetchUserAndDetailsById(int userId)
    {
        _userId = userId;
    }
    
    public override async Task<User?> ExecuteAsync()
    {
        var user =  await QueryFirstOrDefaultAsync<User>(
            new SqlBuilder()
                .Select<User>(s =>
                {
                    s.UsePropertyCase(Casing.SnakeCase);
                    s.RemoveMultipleProperties(User.NotMapped());
                })
                .From("users")
                .Where("id = @Id")
                .Build(), new { Id = _userId });

        if (user is null)
        {
            return null;
        }

        var userDetails = await QueryFirstOrDefaultAsync<UserDetails>(new SqlBuilder()
            .Select<UserDetails>(s =>
            {
                s.UsePropertyCase(Casing.SnakeCase);
            }).From("user_details")
            .Where("user_id = @UserId").Build(), new { UserId = _userId });

        user.UserDetails = userDetails;

        return user;
    }
}