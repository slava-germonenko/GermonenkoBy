using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Common.EntityFramework.Extensions;
using GermonenkoBy.Contacts.Core.Contracts;
using GermonenkoBy.Contacts.Core.Dtos;
using GermonenkoBy.Contacts.Core.Models;

namespace GermonenkoBy.Contacts.Infrastructure.Repos;

public class ContactsRepository : IContactsRepository
{
    private readonly ContactsContext _context;

    private readonly MapperConfiguration _mapperConfiguration = new(options =>
    {
        options.CreateProjection<Entities.Contact, Contact>();
    });

    public ContactsRepository(ContactsContext context)
    {
        _context = context;
    }

    public async Task<Contact?> GetContactAsync(int contactId)
        => await _context.Contacts
            .AsNoTracking()
            .Include(c => c.EmailAddresses)
            .FirstOrDefaultAsync(c => c.Id == contactId);

    public async Task<Contact> SaveContactAsync(Contact contact)
    {
        var contactToSave = await _context.Contacts.FindAsync(contact.Id) ?? new Entities.Contact();

        var phoneIsInUse = await _context.Contacts.AnyAsync(
            c => c.Id != contact.Id && c.PhoneNumber == contact.PhoneNumber
        );

        if (phoneIsInUse)
        {
            throw new InfrastructureException($"Номер телефона {contact.PhoneNumber} уже используется.");
        }

        contactToSave.PhoneNumber = contact.PhoneNumber;
        contactToSave.FirstName = contact.FirstName;
        contactToSave.LastName = contact.LastName;
        contactToSave.LastActivityDate = contact.LastActivityDate;

        _context.Contacts.Update(contactToSave);
        _context.Entry(contactToSave).Collection(c => c.EmailAddresses).IsModified = false;
        await _context.SaveChangesAsync();

        return contactToSave;
    }

    public async Task<PagedSet<Contact>> GetContactsListAsync(ContactsFilterDto filter)
    {
        var query = _context.Contacts
            .Include(c => c.EmailAddresses)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(filter.Phone))
        {
            query = query.Where(c => c.PhoneNumber.Equals(filter.Phone));
        }

        var results = await query.ProjectTo<Contact>(_mapperConfiguration).ToPagedSetAsync(filter);
        return results;
    }

    public async Task AddContactEmails(int contactId, IEnumerable<string> emails)
    {
        var contactExists = await _context.Contacts.AnyAsync(c => c.Id == contactId);
        if (!contactExists)
        {
            throw new InfrastructureException(
                $"Невозможно добавить почтовые адреса контакту: контакт с идентификатором {contactId} не найден."
            );
        }

        var previouslyAddedEmails = await _context.ContactEmailAddresses
            .AsNoTracking()
            .Where(ea => ea.ContactId == contactId)
            .Select(ea => ea.Email)
            .ToListAsync();

        var newContactEmails = emails.Distinct(StringComparer.OrdinalIgnoreCase)
            .Where(e => !previouslyAddedEmails.Contains(e, StringComparer.OrdinalIgnoreCase))
            .Select(e => new ContactEmailAddress
            {
                ContactId = contactId,
                Email = e,
                CreatedDate = DateTime.UtcNow,
                LastUsedDate = DateTime.UtcNow,
            });

        _context.ContactEmailAddresses.AddRange(newContactEmails);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveContactEmails(int contactId, IEnumerable<string> emails)
    {
        var emailsToRemove = await _context.ContactEmailAddresses
            .Where(ea => ea.ContactId == contactId && emails.Contains(ea.Email))
            .ToListAsync();

        _context.ContactEmailAddresses.RemoveRange(emailsToRemove);
        await _context.SaveChangesAsync();
    }
}