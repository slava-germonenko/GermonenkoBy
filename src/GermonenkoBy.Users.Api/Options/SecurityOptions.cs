using GermonenkoBy.Common.Utils.Hashing;

namespace GermonenkoBy.Users.Api.Options;

public class SecurityOptions : HashingOptions
{
    public int MinPasswordLength { get; set; }
}