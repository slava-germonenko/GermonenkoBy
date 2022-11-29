using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.EntityFramework;
using GermonenkoBy.Contacts.Core.Models;
using GermonenkoBy.Contacts.Infrastructure.Entities.Configuration;

namespace GermonenkoBy.Contacts.Infrastructure;

public class ContactsContext : BaseContext
{
    public DbSet<Entities.Contact> Contacts => Set<Entities.Contact>();

    public DbSet<ContactEmailAddress> ContactEmailAddresses => Set<ContactEmailAddress>();

    public ContactsContext(DbContextOptions<ContactsContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ContactEmailAddressEntityTypeConfiguration());
    }
}