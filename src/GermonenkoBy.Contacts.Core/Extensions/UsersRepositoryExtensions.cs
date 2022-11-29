using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Contacts.Core.Contracts;

namespace GermonenkoBy.Contacts.Core.Extensions;

public static class UsersRepositoryExtensions
{
    public static async Task EnsureUserExists(this IUsersRepository usersRepository, int userId)
    {
        var user = await usersRepository.GetUserAsync(userId);
        if (user is null)
        {
            throw new CoreLogicException($"Пользователь с идентификатором \"{userId}\" не найден.");
        }
    }
}