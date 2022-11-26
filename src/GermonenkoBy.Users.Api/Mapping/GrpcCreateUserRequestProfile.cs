using AutoMapper;

using GermonenkoBy.Users.Api.Grpc;
using GermonenkoBy.Users.Core.Dtos;

namespace GermonenkoBy.Users.Api.Mapping;

public class GrpcCreateUserRequestProfile : Profile
{
    public GrpcCreateUserRequestProfile()
    {
        CreateMap<CreateUserRequest, CreateUserDto>();
    }
}