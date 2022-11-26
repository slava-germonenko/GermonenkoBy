using AutoMapper;

using GermonenkoBy.Users.Api.Grpc;
using GermonenkoBy.Users.Api.Mapping.Converters;
using GermonenkoBy.Users.Core.Models;

namespace GermonenkoBy.Users.Api.Mapping;

public class GrpcUserResponseProfile : Profile
{
    public GrpcUserResponseProfile()
    {
        CreateMap<User, UserResponse>()
            .ForMember(resp => resp.UserId, opt => opt.MapFrom(user => user.Id))
            .ForMember(resp => resp.CreatedDate, opt => opt.ConvertUsing(new DateTimeToProtoTimestampConverter()))
            .ForMember(resp => resp.UpdatedDate, opt =>
            {
                opt.PreCondition(user => user.UpdatedDate is not null);
                opt.ConvertUsing(new DateTimeToProtoTimestampConverter());
            });
    }
}