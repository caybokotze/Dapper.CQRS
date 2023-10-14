using System.Threading.Tasks;
using Dapper.CQRS.Tests.TestModels;
using GenericSqlBuilder;

namespace Dapper.CQRS.Tests.Queries;

public class ParallelUserDetailsQuery : QueryAsync<User>
{
    private readonly int _userId;

    public ParallelUserDetailsQuery(int userId)
    {
        _userId = userId;
    }
        
    public override async Task<User> ExecuteAsync()
    {
        var userSql = new SqlBuilder()
            .Select<User>(s =>
            {
                s.UsePropertyCase(Casing.SnakeCase);
                s.RemoveMultipleProperties(User.NotMapped());
            })
            .From("users")
            .Where("id = @Id")
            .Build();
            
        var detailSql = new SqlBuilder()
            .Select<UserDetail>(s =>
            {
                s.UsePropertyCase(Casing.SnakeCase);
            })
            .From("user_details")
            .Where("user_id = @Id")
            .Build();

        var user = Task.Run(() => QueryFirstAsync<User>(userSql, new { Id = _userId }));
        var userDetails = Task.Run(() => QueryFirstAsync<UserDetail>(detailSql, new { Id = _userId }));

        await Task.WhenAll(user, userDetails);

        var userResult = user.Result;
        userResult.UserDetails = userDetails.Result;

        return userResult;
    }
}