using AutoMapper;
using GermonenkoBy.Users.Api.Grpc;

namespace GermonenkoBy.Gateway.Api.Mapping.Converters.Users;

public class UserOrderByConverter : IValueConverter<string?, OrderBy>
{
    public OrderBy Convert(string? orderBy, ResolutionContext context) => orderBy?.ToLower() switch
    {
        "emailaddress" => OrderBy.EmailAddress,
        "firstname" => OrderBy.FirstName,
        "lastname" => OrderBy.LastName,
        "updateddate" => OrderBy.LastName,
        _ => OrderBy.CreatedDate
    };
}