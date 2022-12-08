using GermonenkoBy.Gateway.Api.Models.Contacts;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients;

public interface IContactsClient
{
    public Task<Contact> CreateContactAsync(CreateContactDto contactDto);
}