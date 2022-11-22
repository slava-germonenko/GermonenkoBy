using AutoMapper;

using GermonenkoBy.Gateway.Api.Models.Users;
using GermonenkoBy.Users.Api.Grpc;

namespace GermonenkoBy.Gateway.Api.MapperProfiles.Users;

public class GrpcUpdateUserRequest : Profile
{
    public GrpcUpdateUserRequest()
    {
        CreateMap<ModifyUserDto, UpdateUserRequest>()
            .ForMember(req => req.Active, opt => opt.MapFrom(dto => dto.Active))
            .ForMember(req => req.EmailAddress, opt => opt.MapFrom(dto => dto.EmailAddress))
            .ForMember(req => req.FirstName, opt => opt.MapFrom(dto => dto.FirstName))
            .ForMember(req => req.LastName, opt => opt.MapFrom(dto => dto.LastName));
    }
}