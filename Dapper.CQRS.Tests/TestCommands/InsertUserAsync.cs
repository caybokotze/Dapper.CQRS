using System.Threading.Tasks;
using Dapper.CQRS.Tests.TestModels;
using GenericSqlBuilder;

namespace Dapper.CQRS.Tests.TestCommands;

public class InsertUserAsync : CommandAsync<int>
{
    private readonly User _user;

    public InsertUserAsync(User user)
    {
        _user = user;
    }

    public override Task<int> ExecuteAsync()
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

        return QueryFirstAsync<int>(insertSql, _user);
    }
}