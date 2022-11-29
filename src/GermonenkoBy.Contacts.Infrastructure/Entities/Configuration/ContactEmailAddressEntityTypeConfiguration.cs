using GermonenkoBy.Contacts.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GermonenkoBy.Contacts.Infrastructure.Entities.Configuration;

public class ContactEmailAddressEntityTypeConfiguration
    : IEntityTypeConfiguration<ContactEmailAddress>
{
    public void Configure(EntityTypeBuilder<ContactEmailAddress> builder)
    {
        builder.HasKey(ea => new { ea.ContactId, ea.Email });
    }
}