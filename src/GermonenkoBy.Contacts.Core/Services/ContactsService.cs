using GermonenkoBy.Contacts.Core.Contracts;
using GermonenkoBy.Contacts.Core.Dtos;
using GermonenkoBy.Contacts.Core.Extensions;
using GermonenkoBy.Contacts.Core.Models;

namespace GermonenkoBy.Contacts.Core.Services;

public class ContactsService
{
    private readonly IContactsRepository _contactsRepository;

    private readonly IUsersRepository _usersRepository;

    public ContactsService(
        IContactsRepository contactsRepository,
        IUsersRepository usersRepository
    )
    {
        _contactsRepository = contactsRepository;
        _usersRepository = usersRepository;
    }

    public async Task<Contact> CreateContactAsync(CreateContactDto contactDto)
    {
        await _contactsRepository.EnsurePhoneNumberIsNotInUse(contactDto.PhoneNumber);

        if (contactDto.AssigneeId is not null)
        {
            await _usersRepository.EnsureUserExists(contactDto.AssigneeId.Value);
        }

        var contact = new Contact
        {
            DeletedDate = null,
            AssigneeId = contactDto.AssigneeId,
            PhoneNumber = contactDto.PhoneNumber,
            FirstName = contactDto.FirstName,
            LastName = contactDto.LastName,
        };

        if (contactDto.MarkAsActivity)
        {
            contact.LastActivityDate = DateTime.UtcNow;
        }

        contact = await _contactsRepository.SaveContactAsync(contact);
        await _contactsRepository.AddContactEmails(contact.Id, contactDto.EmailAddresses);

        contact = await _contactsRepository.GetContactAsync(contact.Id);
        return contact!;
    }
}