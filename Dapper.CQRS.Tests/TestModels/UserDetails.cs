using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dapper.CQRS.Tests.TestModels
{
    public class UserDetails
    {
        [Key]
        public int Id { get; set; }
        public string? IdNumber { get; set; }
        
        public int UserId { get; set; }
    }
}