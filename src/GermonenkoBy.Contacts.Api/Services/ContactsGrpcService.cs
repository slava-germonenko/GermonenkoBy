using System.ComponentModel.DataAnnotations;

using AutoMapper;
using Grpc.Core;

using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Contacts.Core.Dtos;

using ContactsCoreService = GermonenkoBy.Contacts.Core.Services.ContactsService;
using GrpcServiceBase = GermonenkoBy.Contacts.Api.ContactsService.ContactsServiceBase;

namespace GermonenkoBy.Contacts.Api.Services;

public class ContactsGrpcService : GrpcServiceBase
{
    private readonly ContactsCoreService _contactsService;

    private readonly IMapper _mapper;

    public ContactsGrpcService(ContactsCoreService contactsService, IMapper mapper)
    {
        _contactsService = contactsService;
        _mapper = mapper;
    }

    public override async Task<ContactResponse> CreateContact(CreateContactRequest request, ServerCallContext context)
    {
        try
        {
            var userDto = _mapper.Map<CreateContactDto>(request);
            EnsureModelIsValid(userDto);
            var user = await _contactsService.CreateContactAsync(userDto);
            return _mapper.Map<ContactResponse>(user);
        }
        catch (CoreLogicException e)
        {
            context.Status = new Status(StatusCode.InvalidArgument, e.Message);
            return new ContactResponse();
        }
        catch (InfrastructureException e)
        {
            context.Status = new Status(StatusCode.InvalidArgument, e.Message);
            return new ContactResponse();
        }
        catch (ValidationException e)
        {
            context.Status = new Status(StatusCode.InvalidArgument, e.Message);
            return new ContactResponse();
        }
    }

    private static void EnsureModelIsValid<TModel>(TModel model)
    {
        if (model != null)
        {
            Validator.ValidateObject(model, new ValidationContext(model));
        }
    }
}