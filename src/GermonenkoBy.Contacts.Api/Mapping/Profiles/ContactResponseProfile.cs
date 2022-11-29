using AutoMapper;
using Google.Protobuf.WellKnownTypes;

using GermonenkoBy.Contacts.Core.Models;

namespace GermonenkoBy.Contacts.Api.Mapping.Profiles;

public class ContactResponseProfile : Profile
{
    public ContactResponseProfile()
    {
        CreateMap<Contact, ContactResponse>()
            .ForMember(resp => resp.CreatedDate, opt => opt.MapFrom(c => Timestamp.FromDateTime(c.CreatedDate)))
            .ForMember(resp => resp.UpdatedDate, opt =>
            {
                opt.PreCondition(c => c.UpdatedDate is not null);
                opt.MapFrom(c => Timestamp.FromDateTime(c.UpdatedDate!.Value));
            })
            .ForMember(resp => resp.DeletedDate, opt =>
            {
                opt.PreCondition(c => c.DeletedDate is not null);
                opt.MapFrom(c => Timestamp.FromDateTime(c.DeletedDate!.Value));
            })
            .ForMember(resp => resp.LastActivityDate, opt =>
            {
                opt.PreCondition(c => c.LastActivityDate is not null);
                opt.MapFrom(c => Timestamp.FromDateTime(c.LastActivityDate!.Value));
            })
            .ForMember(resp => resp.Emails, opt => opt.MapFrom(c => c.EmailAddresses));
    }
}