using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Common.Utils.Hashing;

namespace GermonenkoBy.Users.Core;

public class PasswordValidationService
{
    private readonly UsersContext _context;

    private readonly IHasher _hasher;

    public PasswordValidationService(UsersContext context, IHasher hasher)
    {
        _context = context;
        _hasher = hasher;
    }

    public async Task<bool> PasswordIsValidAsync(int userId, string password)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user is null)
        {
            throw new NotFoundException($"Пользователь с идентификатором \"{userId}\" не найден.");
        }

        var hashToBeValidated = _hasher.GetHash(password, user.PasswordSalt);
        return hashToBeValidated.Equals(user.PasswordHash);
    }
}