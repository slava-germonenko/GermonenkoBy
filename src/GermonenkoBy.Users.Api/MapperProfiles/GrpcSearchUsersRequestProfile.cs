using AutoMapper;

using GermonenkoBy.Users.Api.Grpc;
using GermonenkoBy.Users.Core.Dtos;

namespace GermonenkoBy.Users.Api.MapperProfiles;

public class GrpcSearchUsersRequestProfile : Profile
{
    public GrpcSearchUsersRequestProfile()
    {
        CreateMap<SearchUsersRequest, UsersFilterDto>()
            .ForMember(filter => filter.Search, source => source.MapFrom(req => req.Search))
            .ForMember(filter => filter.Count, source => source.MapFrom(req => req.Count))
            .ForMember(filter => filter.Offset, source => source.MapFrom(req => req.Offset))
            .ForMember(filter => filter.EmailAddress, source => source.MapFrom(req => req.EmailAddress))
            .ForMember(filter => filter.Active, source => source.MapFrom(req => MapActive(req.Active)))
            .ForMember(filter => filter.FirstName, source => source.MapFrom(req => req.FirstName))
            .ForMember(filter => filter.LastName, source => source.MapFrom(req => req.LastName))
            .ForMember(
                filter => filter.OrderDirection,
                source => source.MapFrom(req => req.OrderDirection == OrderDirection.Asc ? "asc" : "desc")
            )
            .ForMember(
                filter => filter.OrderBy,
                source => source.MapFrom(req => MapOrderByField(req.OrderBy))
            );
    }

    private static bool? MapActive(UserStatus status) => status switch
    {
        UserStatus.Active => true,
        UserStatus.Inactive => false,
        _ => null,
    };

    private static string MapOrderByField(OrderBy orderBy) => orderBy switch
    {
        OrderBy.EmailAddress => "emailaddress",
        OrderBy.FirstName => "firstname",
        OrderBy.LastName => "lastname",
        OrderBy.UpdatedDate => "updateddate",
        _ => "createddate"
    };
}