using System.Threading.Tasks;
using Dapper.CQRS.Tests.TestModels;
using GenericSqlBuilder;

namespace Dapper.CQRS.Tests.Commands
{
    public class InsertUserType : Command<int>
    {
        public UserType UserType { get; }

        public InsertUserType(UserType userType)
        {
            UserType = userType;
        }
        
        public override Task<int> Execute()
        {
            var sql = new SqlBuilder()
                .Insert<UserType>("user_types")
                .Values()
                .AppendStatement()
                .Select()
                .LastInserted(Version.MySql);

            return QueryFirst<int>(sql);
        }
    }
}