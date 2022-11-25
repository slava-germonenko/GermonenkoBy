using AutoMapper;

using GermonenkoBy.Users.Api.Grpc;

namespace GermonenkoBy.Gateway.Api.Mapping.Converters.Users;

public class OrderByDirectionConverter : IValueConverter<string?, OrderDirection>
{
    public OrderDirection Convert(string? orderDirection, ResolutionContext context)
    {
        if (orderDirection is null)
        {
            return OrderDirection.Asc;
        }

        return orderDirection.Equals("desc", StringComparison.OrdinalIgnoreCase)
            ? OrderDirection.Desc
            : OrderDirection.Asc;
    }
}