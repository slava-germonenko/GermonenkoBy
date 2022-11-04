using GermonenkoBy.Common.Domain;

namespace GermonenkoBy.Gateway.Api.Models.Users;

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