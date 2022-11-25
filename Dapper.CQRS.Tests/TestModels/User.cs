using System.Collections.Generic;

namespace Dapper.CQRS.Tests.TestModels
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public virtual UserType UserType { get; set; }
        public virtual UserDetails UserDetails { get; set; }

        public static IEnumerable<string> NotMapped()
        {
            yield return nameof(UserType);
            yield return nameof(UserDetails);
        }
    }
}