using System.Threading.Tasks;
using Dapper.CQRS.Tests.TestModels;
using GenericSqlBuilder;

namespace Dapper.CQRS.Tests.TestCommands;

public class InsertUserDetailAsync : CommandAsync<int>
{
    private readonly UserDetails _userDetails;

    public InsertUserDetailAsync(UserDetails userDetails)
    {
        _userDetails = userDetails;
    }
    
    public override Task<int> ExecuteAsync()
    {
        var sql = new SqlBuilder()
            .Insert<UserDetails>("user_details", i =>
            {
                i.RemoveProperty(nameof(UserDetails.Id));
                i.UsePropertyCase(Casing.SnakeCase);
            })
            .Values()
            .AppendStatement()
            .Select()
            .LastInserted(Version.MySql);

        return QueryFirstAsync<int>(sql, _userDetails);
    }

}