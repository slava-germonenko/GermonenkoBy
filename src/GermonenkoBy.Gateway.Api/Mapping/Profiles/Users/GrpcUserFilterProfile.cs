using AutoMapper;

using GermonenkoBy.Gateway.Api.Mapping.Converters.Users;
using GermonenkoBy.Gateway.Api.Models.Users;
using GermonenkoBy.Users.Api.Grpc;

namespace GermonenkoBy.Gateway.Api.Mapping.Profiles.Users;

public class GrpcUserFilterProfile : Profile
{
    public GrpcUserFilterProfile()
    {
        CreateMap<UsersFilterDto, SearchUsersRequest>()
            .ForMember(request => request.Search, opt => opt.Condition(filter => !string.IsNullOrEmpty(filter.Search)))
            .ForMember(request => request.EmailAddress, opt => opt.Condition(filter => !string.IsNullOrEmpty(filter.EmailAddress)))
            .ForMember(request => request.Active, source => source.ConvertUsing(new UserStatusConverter()))
            .ForMember(request => request.FirstName, opt => opt.Condition(filter => !string.IsNullOrEmpty(filter.FirstName)))
            .ForMember(request => request.LastName, opt => opt.Condition(filter => !string.IsNullOrEmpty(filter.LastName)))
            .ForMember(request => request.OrderDirection, source => source.ConvertUsing(new OrderByDirectionConverter()))
            .ForMember(request => request.OrderBy, source => source.ConvertUsing(new UserOrderByConverter()));
    }
}