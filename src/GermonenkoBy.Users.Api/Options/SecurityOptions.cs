using GermonenkoBy.Common.Utils.Hashing;

namespace GermonenkoBy.Users.Api.Options;

public class SecurityOptions : HashingOptions
{
    public int MinPasswordLenght { get; set; }
}