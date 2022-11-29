using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Contacts.Core.Contracts;
using GermonenkoBy.Contacts.Core.Models;

namespace GermonenkoBy.Contacts.Core.Extensions;

public static class ContactsRepositoryExtensions
{
    public static async Task<Contact?> GetContactByPhoneNumber(
        this IContactsRepository contactsRepository,
        string phone
    )
    {
        var contacts = await contactsRepository.GetContactsListAsync(new()
        {
            Count = 1,
            Phone = phone,
        });

        return contacts.Data.FirstOrDefault();
    }

    public static async Task EnsurePhoneNumberIsNotInUse(
        this IContactsRepository contactsRepository,
        string phone
     )
    {
        var contact = await contactsRepository.GetContactByPhoneNumber(phone);
        if (contact is not null)
        {
            throw new CoreLogicException($"Номер телефона {phone} уже используется.");
        }
    }
}