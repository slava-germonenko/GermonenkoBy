using AutoMapper;

using GermonenkoBy.Gateway.Api.Models.Users;
using GermonenkoBy.Users.Api.Grpc;

namespace GermonenkoBy.Gateway.Api.Mapping.Profiles.Users;

public class GrpcCreateUserRequestProfile : Profile
{
    public GrpcCreateUserRequestProfile()
    {
        CreateMap<CreateUserDto, CreateUserRequest>()
            .ForMember(req => req.Active, opt => opt.MapFrom(dto => dto.Active))
            .ForMember(req => req.EmailAddress, opt => opt.MapFrom(dto => dto.EmailAddress))
            .ForMember(req => req.FirstName, opt => opt.MapFrom(dto => dto.FirstName))
            .ForMember(req => req.LastName, opt => opt.MapFrom(dto => dto.LastName))
            .ForMember(req => req.Password, opt => opt.MapFrom(dto => dto.Password));
    }
}