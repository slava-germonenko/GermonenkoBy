using AutoMapper;

using GermonenkoBy.Contacts.Api.Grpc;
using GermonenkoBy.Gateway.Api.Models.Contacts;

namespace GermonenkoBy.Gateway.Api.Mapping.Profiles.Contacts;

public class CreateContactDtoProfile : Profile
{
    public CreateContactDtoProfile()
    {
        CreateMap<CreateContactDto, CreateContactRequest>()
            .ForMember(req => req.AssigneeId, opt => opt.PreCondition(dto => dto.AssigneeId is not null));
    }
}