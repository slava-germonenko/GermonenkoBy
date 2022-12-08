using AutoMapper;

using GermonenkoBy.Contacts.Api.Grpc;
using GermonenkoBy.Contacts.Core.Models;
using GermonenkoBy.Contacts.Api.Mapping.Converters;

namespace GermonenkoBy.Contacts.Api.Mapping.Profiles;

public class ContactResponseProfile : Profile
{
    public ContactResponseProfile()
    {
        CreateMap<Contact, ContactResponse>()
            .ForMember(resp => resp.CreatedDate, opt =>
            {
                opt.ConvertUsing(new DateTimeToTimestampConverter());
            })
            .ForMember(resp => resp.UpdatedDate, opt =>
            {
                opt.PreCondition(c => c.UpdatedDate is not null);
                opt.ConvertUsing(new DateTimeToTimestampConverter());
            })
            .ForMember(resp => resp.DeletedDate, opt =>
            {
                opt.PreCondition(c => c.DeletedDate is not null);
                opt.ConvertUsing(new DateTimeToTimestampConverter());
            })
            .ForMember(resp => resp.LastActivityDate, opt =>
            {
                opt.PreCondition(c => c.LastActivityDate is not null);
                opt.ConvertUsing(new DateTimeToTimestampConverter());
            })
            .ForMember(resp => resp.Emails, opt => opt.MapFrom(c => c.EmailAddresses));
    }
}