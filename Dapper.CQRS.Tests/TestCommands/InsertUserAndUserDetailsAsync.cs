using System.Threading.Tasks;
using Dapper.CQRS.Tests.TestModels;

namespace Dapper.CQRS.Tests.TestCommands;

public class InsertUserAndUserDetailsAsync : CommandAsync
{
    private readonly User _user;
    private readonly UserDetails _userDetails;

    public InsertUserAndUserDetailsAsync(User user, UserDetails userDetails)
    {
        _user = user;
        _userDetails = userDetails;
    }

    public override async Task ExecuteAsync()
    {
        var userId = await CommandExecutor.ExecuteAsync(new InsertUserAsync(_user));
        _userDetails.UserId = userId;
        await CommandExecutor.ExecuteAsync(new InsertUserDetailAsync(_userDetails));
    }
}