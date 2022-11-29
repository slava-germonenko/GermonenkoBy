using GermonenkoBy.Common.Domain;
using GermonenkoBy.Contacts.Core.Dtos;
using GermonenkoBy.Contacts.Core.Models;

namespace GermonenkoBy.Contacts.Core.Contracts;

public interface IContactsRepository
{
    public Task<Contact?> GetContactAsync(int contactId);

    public Task<Contact> SaveContactAsync(Contact contact);

    public Task<PagedSet<Contact>> GetContactsListAsync(ContactsFilterDto filter);

    public Task AddContactEmails(int contactId, IEnumerable<string> emails);

    public Task RemoveContactEmails(int contactId, IEnumerable<string> emails);
}