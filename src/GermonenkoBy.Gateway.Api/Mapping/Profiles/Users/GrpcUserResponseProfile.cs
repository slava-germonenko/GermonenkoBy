using AutoMapper;

using GermonenkoBy.Gateway.Api.Models.Users;
using GermonenkoBy.Users.Api.Grpc;

namespace GermonenkoBy.Gateway.Api.Mapping.Profiles.Users;

public class GrpcUserResponseProfile : Profile
{
    public GrpcUserResponseProfile()
    {
        CreateMap<UserResponse, User>()
            .ForMember(user => user.Id, source => source.MapFrom(response => response.UserId))
            .ForMember(user => user.EmailAddress, source => source.MapFrom(response => response.EmailAddress))
            .ForMember(user => user.FirstName, source => source.MapFrom(response => response.FirstName))
            .ForMember(user => user.LastName, source => source.MapFrom(response => response.LastName))
            .ForMember(user => user.Active, source => source.MapFrom(response => response.Active));
    }
}