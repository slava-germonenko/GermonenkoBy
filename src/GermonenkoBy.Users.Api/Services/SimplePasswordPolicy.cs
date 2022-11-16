using Microsoft.Extensions.Options;

using GermonenkoBy.Users.Api.Options;
using GermonenkoBy.Users.Core.Contracts;

namespace GermonenkoBy.Users.Api.Services;

/// <summary>
/// Simple password policy that can be used for dev purposes.
/// </summary>
public class SimplePasswordPolicy : IPasswordPolicy
{
    private readonly IOptionsSnapshot<SecurityOptions> _securityOptions;

    private int PasswordLen => _securityOptions.Value.MinPasswordLength;

    public string PolicyDescription => $"Пароль должен содержать как минимум {PasswordLen} символов!";

    public bool PasswordMeetsPolicyRequirements(string password)
    {
        return password.Length >= PasswordLen;
    }

    public SimplePasswordPolicy(IOptionsSnapshot<SecurityOptions> securityOptions)
    {
        _securityOptions = securityOptions;
    }
}