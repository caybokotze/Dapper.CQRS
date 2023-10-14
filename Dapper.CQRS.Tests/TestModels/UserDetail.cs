using System.ComponentModel.DataAnnotations;

namespace Dapper.CQRS.Tests.TestModels;

public class UserDetail
{
    [Key]
    public int Id { get; set; }
    public string? IdNumber { get; set; }
        
    public int UserId { get; set; }
}