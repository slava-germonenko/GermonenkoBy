using GermonenkoBy.Common.Domain;

namespace GermonenkoBy.Users.Core.Dtos;

public class UsersFilterDto : Paging
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? EmailAddress { get; set; }

    public bool? Active { get; set; }

    public string? Search { get; set; }

    public string OrderDirection { get; set; } = "ASC";

    public string? OrderBy { get; set; }
}