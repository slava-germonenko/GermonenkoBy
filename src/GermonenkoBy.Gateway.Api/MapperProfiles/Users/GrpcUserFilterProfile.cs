using AutoMapper;

using GermonenkoBy.Gateway.Api.Models.Users;
using GermonenkoBy.Users.Api.Grpc;

namespace GermonenkoBy.Gateway.Api.MapperProfiles.Users;

public class GrpcUserFilterProfile : Profile
{
    public GrpcUserFilterProfile()
    {
        CreateMap<UsersFilterDto, SearchUsersRequest>()
            .ForMember(request => request.Search, source => source.MapFrom(filter => filter.Search))
            .ForMember(request => request.Count, source => source.MapFrom(filter => filter.Count))
            .ForMember(request => request.Offset, source => source.MapFrom(filter => filter.Offset))
            .ForMember(request => request.EmailAddress, source => source.MapFrom(filter => filter.EmailAddress))
            .ForMember(request => request.Active, source => source.MapFrom(filter => MapActive(filter.Active)))
            .ForMember(request => request.FirstName, source => source.MapFrom(filter => filter.FirstName))
            .ForMember(request => request.LastName, source => source.MapFrom(filter => filter.LastName))
            .ForMember(
                request => request.OrderDirection,
                source => source.MapFrom(
                    filter => filter.OrderDirection.Equals("desc", StringComparison.OrdinalIgnoreCase)
                        ? OrderDirection.Desc
                        : OrderDirection.Asc
                )
            )
            .ForMember(
                request => request.OrderBy,
                source => source.MapFrom(filter => MapOrderByField(filter.OrderBy))
            );
    }

    private static UserStatus MapActive(bool? active) => active switch
    {
        true => UserStatus.Active,
        false => UserStatus.Inactive,
        _ => UserStatus.All,
    };

    private static OrderBy MapOrderByField(string? orderBy) => orderBy?.ToLower() switch
    {
        "emailaddress" => OrderBy.EmailAddress,
        "firstname" => OrderBy.FirstName,
        "lastname" => OrderBy.LastName,
        "updateddate" => OrderBy.LastName,
        _ => OrderBy.CreatedDate
    };
}