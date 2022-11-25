using AutoMapper;

using GermonenkoBy.Users.Api.Grpc;

namespace GermonenkoBy.Gateway.Api.Mapping.Converters.Users;

public class UserStatusConverter : IValueConverter<bool?, UserStatus>
{
    public UserStatus Convert(bool? active, ResolutionContext context)
        => active switch
        {
            true => UserStatus.Active,
            false => UserStatus.Inactive,
            _ => UserStatus.All,
        };
}