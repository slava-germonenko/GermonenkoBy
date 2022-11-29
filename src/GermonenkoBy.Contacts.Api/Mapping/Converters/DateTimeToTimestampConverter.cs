using AutoMapper;
using Google.Protobuf.WellKnownTypes;

namespace GermonenkoBy.Contacts.Api.Mapping.Converters;

public class DateTimeToTimestampConverter : IValueConverter<DateTime, Timestamp>
{
    public Timestamp Convert(DateTime dateTime, ResolutionContext context)
        => Timestamp.FromDateTime(dateTime.ToUniversalTime());
}