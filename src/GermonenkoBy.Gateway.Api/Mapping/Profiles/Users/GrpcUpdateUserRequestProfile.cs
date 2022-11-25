using AutoMapper;

using GermonenkoBy.Gateway.Api.Models.Users;
using GermonenkoBy.Users.Api.Grpc;

namespace GermonenkoBy.Gateway.Api.Mapping.Profiles.Users;

public class GrpcUpdateUserRequestProfile : Profile
{
    public GrpcUpdateUserRequestProfile()
    {
        CreateMap<ModifyUserDto, UpdateUserRequest>()
            .ForMember(req => req.Active, opt => opt.MapFrom(dto => dto.Active))
            .ForMember(req => req.EmailAddress, opt => opt.MapFrom(dto => dto.EmailAddress))
            .ForMember(req => req.FirstName, opt => opt.MapFrom(dto => dto.FirstName))
            .ForMember(req => req.LastName, opt => opt.MapFrom(dto => dto.LastName));
    }
}