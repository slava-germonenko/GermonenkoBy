using AutoMapper;

using GermonenkoBy.Users.Api.Grpc;
using GermonenkoBy.Users.Api.Mapping.Converters;
using GermonenkoBy.Users.Core.Dtos;

namespace GermonenkoBy.Users.Api.Mapping;

public class GrpcSearchUsersRequestProfile : Profile
{
    public GrpcSearchUsersRequestProfile()
    {
        CreateMap<SearchUsersRequest, UsersFilterDto>()
            .ForMember(filter => filter.Active, opt => opt.ConvertUsing(new UserStatusToBoolConverter()))
            .ForMember(
                filter => filter.OrderDirection,
                source => source.MapFrom(req => req.OrderDirection.ToString().ToLower())
            )
            .ForMember(
                filter => filter.OrderBy,
                source => source.MapFrom(req => req.OrderBy.ToString().ToLower())
            );
    }
}