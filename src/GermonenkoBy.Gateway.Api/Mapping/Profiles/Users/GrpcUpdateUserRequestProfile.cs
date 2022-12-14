using AutoMapper;

using GermonenkoBy.Gateway.Api.Models.Users;
using GermonenkoBy.Users.Api.Grpc;

namespace GermonenkoBy.Gateway.Api.Mapping.Profiles.Users;

public class GrpcUpdateUserRequestProfile : Profile
{
    public GrpcUpdateUserRequestProfile()
    {
        CreateMap<ModifyUserDto, UpdateUserRequest>();
    }
}