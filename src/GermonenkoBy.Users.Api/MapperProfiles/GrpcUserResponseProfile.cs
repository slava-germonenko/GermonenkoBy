using AutoMapper;

using GermonenkoBy.Users.Api.Grpc;
using GermonenkoBy.Users.Core.Models;

namespace GermonenkoBy.Users.Api.MapperProfiles;

public class GrpcUserResponseProfile : Profile
{
    public GrpcUserResponseProfile()
    {
        var map = CreateMap<User, UserResponse>();
        map.ForMember(userResponse => userResponse.UserId, source => source.MapFrom(user => user.Id))
            .ForMember(userResponse => userResponse.EmailAddress, source => source.MapFrom(user => user.EmailAddress))
            .ForMember(userResponse => userResponse.FirstName, source => source.MapFrom(user => user.FirstName))
            .ForMember(userResponse => userResponse.LastName, source => source.MapFrom(user => user.LastName))
            .ForMember(userResponse => userResponse.Active, source => source.MapFrom(user => user.Active));
    }
}