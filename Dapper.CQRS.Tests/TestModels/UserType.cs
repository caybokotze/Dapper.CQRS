using System.ComponentModel.DataAnnotations.Schema;

namespace Dapper.CQRS.Tests.TestModels
{
    public class UserType
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        
        [ForeignKey(nameof(User.Id))]
        public int UserId { get; set; }
    }
}