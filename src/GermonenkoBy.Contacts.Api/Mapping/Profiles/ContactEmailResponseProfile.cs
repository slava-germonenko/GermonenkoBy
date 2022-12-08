using AutoMapper;

using GermonenkoBy.Contacts.Api.Grpc;
using GermonenkoBy.Contacts.Api.Mapping.Converters;
using GermonenkoBy.Contacts.Core.Models;

namespace GermonenkoBy.Contacts.Api.Mapping.Profiles;

public class ContactEmailResponseProfile : Profile
{
    public ContactEmailResponseProfile()
    {
        CreateMap<ContactEmailAddress, ContactEmailResponse>()
            .ForMember(resp => resp.CreatedDate, opt => opt.ConvertUsing(new DateTimeToTimestampConverter()))
            .ForMember(resp => resp.LastUsedDate, opt => opt.ConvertUsing(new DateTimeToTimestampConverter()));
    }
}