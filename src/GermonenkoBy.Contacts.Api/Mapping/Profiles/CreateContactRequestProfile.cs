using AutoMapper;
using GermonenkoBy.Contacts.Core.Dtos;

namespace GermonenkoBy.Contacts.Api.Mapping.Profiles
{
    public class CreateContactRequestProfile : Profile
    {
        public CreateContactRequestProfile()
        {
            CreateMap<CreateContactRequest, CreateContactDto>()
                .ForMember(
                    dto => dto.EmailAddresses,
                    opt => opt.MapFrom(req => req.Emails.ToList())
                )
                .ForMember(
                    dto => dto.AssigneeId, 
                    opt => opt.PreCondition(req => req.AssigneeId != default)
                );
        }
    }
}
