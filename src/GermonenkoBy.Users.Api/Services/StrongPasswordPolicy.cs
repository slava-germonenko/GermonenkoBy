using Microsoft.Extensions.Options;

using GermonenkoBy.Users.Api.Options;
using GermonenkoBy.Users.Core.Contracts;

namespace GermonenkoBy.Users.Api.Services;

/// <summary>
/// Strong password policy that can be used for prod purposes.
/// </summary>
public class StrongPasswordPolicy : IPasswordPolicy
{
    private readonly IOptionsSnapshot<SecurityOptions> _securityOptions;

    private int PasswordLen => _securityOptions.Value.MinPasswordLenght;

    public string PolicyDescription => $"Пароль должен содержать как минимум {PasswordLen} символов, " +
                                       "хотя бы одну цифру и одну заглавную букву, а также специальный символ!";

    public bool PasswordMeetsPolicyRequirements(string password)
    {
        return password.Length >= PasswordLen
               && password.Any(char.IsUpper)
               && password.Any(char.IsLower)
               && password.Any(char.IsNumber)
               && password.Any(char.IsPunctuation);
    }

    public StrongPasswordPolicy(IOptionsSnapshot<SecurityOptions> securityOptions)
    {
        _securityOptions = securityOptions;
    }
}