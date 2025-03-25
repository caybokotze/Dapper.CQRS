using System.Threading.Tasks;
using Dapper.CQRS.Tests.TestModels;
using Dapper.CQRS.Tests.TestQueries;

namespace Dapper.CQRS.Tests.TestCommands;


public class InsertUserIfNotExists : CommandAsync<User>
{
    private readonly User _user;

    public InsertUserIfNotExists(User user)
    {
        _user = user;
    }
    
    public override async Task<User> ExecuteAsync()
    {
        var user = await QueryExecutor.ExecuteAsync(new FetchUserByIdAsync(_user.Id));
        
        if (user is not null)
        {
            return user;
        }
        
        var userId = CommandExecutor.Execute(new InsertUser(_user));
        _user.Id = userId;
        return _user;
    }
}