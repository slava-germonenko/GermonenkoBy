using GermonenkoBy.UserTermination.Core.Repositories;

namespace GermonenkoBy.UserTermination.Core;

public class UserTerminationService
{
    private readonly IUsersRepository _usersRepository;

    private readonly IUserSessionsRepository _userSessionsRepository;

    public UserTerminationService(IUsersRepository usersRepository, IUserSessionsRepository userSessionsRepository)
    {
        _usersRepository = usersRepository;
        _userSessionsRepository = userSessionsRepository;
    }

    public async Task TerminateAsync(int userId)
    {
        var user = await _usersRepository.GetUserAsync(userId);
        if (user is null)
        {
            return;
        }

        var userSessions = await _userSessionsRepository.GetUserSessionsAsync(userId);
        var removeUserSessionTasks = new List<Task>(userSessions.Count);

        foreach (var session in userSessions)
        {
            var removeTask = _userSessionsRepository.RemoveSessionAsync(session.Id);
            removeUserSessionTasks.Add(removeTask);
        }

        await Task.WhenAll(removeUserSessionTasks);
        await _usersRepository.RemoveUserAsync(userId);
    }
}