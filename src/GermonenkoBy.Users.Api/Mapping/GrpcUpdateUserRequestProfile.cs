using AutoMapper;

using GermonenkoBy.Users.Api.Grpc;
using GermonenkoBy.Users.Core.Dtos;

namespace GermonenkoBy.Users.Api.Mapping;

public class GrpcUpdateUserRequestProfile : Profile
{
    public GrpcUpdateUserRequestProfile()
    {
        CreateMap<UpdateUserRequest, ModifyUserDto>();
    }
}