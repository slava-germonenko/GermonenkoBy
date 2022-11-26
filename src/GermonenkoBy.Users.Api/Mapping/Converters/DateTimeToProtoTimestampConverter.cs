using AutoMapper;

using Google.Protobuf.WellKnownTypes;

namespace GermonenkoBy.Users.Api.Mapping.Converters;

public class DateTimeToProtoTimestampConverter : IValueConverter<DateTime, Timestamp>
{
    public Timestamp Convert(DateTime dateTime, ResolutionContext context)
        => new()
        {
            Seconds = dateTime.ToUniversalTime().ToTimestamp().Seconds,
        };
}