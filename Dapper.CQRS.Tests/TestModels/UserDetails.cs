using System.ComponentModel.DataAnnotations.Schema;

namespace Dapper.CQRS.Tests.TestModels
{
    public class UserDetails
    {
        public string IdNumber { get; set; }

        [ForeignKey(nameof(User.Id))]
        public int UserId { get; set; }
    }
}