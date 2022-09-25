namespace GermonenkoBy.Users.Core.Contracts;

public interface IPasswordPolicy
{
    public string PolicyDescription { get; }

    public bool PasswordMeetsPolicyRequirements(string password);
}