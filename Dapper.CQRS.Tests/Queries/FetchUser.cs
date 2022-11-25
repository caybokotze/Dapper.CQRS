using System.Collections.Generic;
using System.Linq;
using Dapper.CQRS.Tests.TestModels;
using GenericSqlBuilder;

namespace Dapper.CQRS.Tests.Queries
{
    public class FetchUser : Query<User?>
    {
        public int Id { get; }

        public FetchUser(int id)
        {
            Id = id;
        }
        
        public override User? Execute()
        {
            var users =  QueryList<User>(
                new SqlBuilder()
                .Select<User>(s =>
                {
                    s.UsePropertyCase(Casing.SnakeCase);
                })
                .From("users")
                .Build());

            return users.FirstOrDefault();
        }
    }
}