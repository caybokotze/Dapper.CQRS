using Dapper.CQRS.Tests.TestModels;
using GenericSqlBuilder;

namespace Dapper.CQRS.Tests.Commands;

public class InsertUserDetail : Command<int>
{
    private readonly UserDetail _userDetail;

    public InsertUserDetail(UserDetail userDetail)
    {
        _userDetail = userDetail;
    }
    
    public override int Execute()
    {
        var sql = new SqlBuilder()
            .Insert<UserDetail>("user_details", i =>
            {
                i.RemoveProperty(nameof(UserDetail.Id));
                i.UsePropertyCase(Casing.SnakeCase);
            })
            .Values()
            .AppendStatement()
            .Select()
            .LastInserted(Version.MySql);

        return QueryFirst<int>(sql, _userDetail);
    }
}