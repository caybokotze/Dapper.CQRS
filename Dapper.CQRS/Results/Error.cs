namespace Dapper.CQRS.Results;

public class Error
{
    public Error(string details) : this(null, details)
    {
            
    }

    public Error(string? code, string details)
    {
        Code = code;
        Details = details;
    }

    public string? Code { get; set; }
    public string Details { get; set; }
}