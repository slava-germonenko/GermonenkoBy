using GermonenkoBy.Common.Domain;

namespace GermonenkoBy.Contacts.Core.Dtos;

public class ContactsFilterDto : Paging
{
    public string? Phone { get; set; }
}