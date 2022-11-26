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
            .ForMember(user => user.CreatedDate, opt => opt.MapFrom(resp => resp.CreatedDate.ToDateTime()))
            .ForMember(user => user.UpdatedDate, opt =>
            {
                opt.PreCondition(resp => resp.UpdatedDate != default);
                opt.MapFrom(resp => resp.UpdatedDate.ToDateTime());
            });
    }
}