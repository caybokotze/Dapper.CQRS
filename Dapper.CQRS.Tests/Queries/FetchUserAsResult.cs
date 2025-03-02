using System.Linq;
using Dapper.CQRS.Results;
using Dapper.CQRS.Tests.TestModels;
using GenericSqlBuilder;

namespace Dapper.CQRS.Tests.Queries;

public class FetchUserAsResult : Query<Result<User>>
{
    public int Id { get; }

    public FetchUserAsResult(int id)
    {
        Id = id;
    }
        
    public override Result<User> Execute()
    {
        var users =  QueryList<User>(
            new SqlBuilder()
                .Select<User>(s =>
                {
                    s.UsePropertyCase(Casing.SnakeCase);
                    s.RemoveMultipleProperties(User.NotMapped());
                })
                .From("users")
                .Build());

        var user = users.FirstOrDefault();

        if (user is null)
        {
            return new ErrorResult<User>("The user can not be found");
        }

        return new SuccessResult<User>(user);
    }
}