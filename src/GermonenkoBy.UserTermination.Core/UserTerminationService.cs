using GermonenkoBy.UserTermination.Core.Clients;

namespace GermonenkoBy.UserTermination.Core;

public class UserTerminationService
{
    private readonly IUsersClient _usersClient;

    private readonly IUserSessionsClient _userSessionsClient;

    public UserTerminationService(IUsersClient usersClient, IUserSessionsClient userSessionsClient)
    {
        _usersClient = usersClient;
        _userSessionsClient = userSessionsClient;
    }

    public async Task TerminateAsync(int userId)
    {
        var user = await _usersClient.GetUserAsync(userId);
        if (user is null)
        {
            return;
        }

        var userSessions = await _userSessionsClient.GetUserSessionsAsync(userId);
        var removeUserSessionTasks = new List<Task>(userSessions.Count);

        foreach (var session in userSessions)
        {
            var removeTask = _userSessionsClient.RemoveSessionAsync(session.Id);
            removeUserSessionTasks.Add(removeTask);
        }

        await Task.WhenAll(removeUserSessionTasks);
        await _usersClient.RemoveUserAsync(userId);
    }
}