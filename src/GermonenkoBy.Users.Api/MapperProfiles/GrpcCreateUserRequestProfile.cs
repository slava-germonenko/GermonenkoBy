using AutoMapper;

using GermonenkoBy.Users.Api.Grpc;
using GermonenkoBy.Users.Core.Dtos;

namespace GermonenkoBy.Users.Api.MapperProfiles;

public class GrpcCreateUserRequestProfile : Profile
{
    public GrpcCreateUserRequestProfile()
    {
        CreateMap<CreateUserRequest, CreateUserDto>()
            .ForMember(userDto => userDto.EmailAddress, source => source.MapFrom(req => req.EmailAddress))
            .ForMember(userDto => userDto.FirstName, source => source.MapFrom(req => req.FirstName))
            .ForMember(userDto => userDto.LastName, source => source.MapFrom(req => req.LastName))
            .ForMember(userDto => userDto.Active, source => source.MapFrom(req => req.Active))
            .ForMember(userDto => userDto.Password, source => source.MapFrom(req => req.Password));
    }
}