using AutoMapper;

using GermonenkoBy.Users.Api.Grpc;

namespace GermonenkoBy.Users.Api.Mapping.Converters;

public class UserStatusToBoolConverter : IValueConverter<UserStatus, bool?>
{
    public bool? Convert(UserStatus status, ResolutionContext context) => status switch
    {
        UserStatus.Active => true,
        UserStatus.Inactive => false,
        _ => null
    };
}