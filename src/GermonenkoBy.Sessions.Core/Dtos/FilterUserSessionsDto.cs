using GermonenkoBy.Common.Domain;

namespace GermonenkoBy.Sessions.Core.Dtos;

public class FilterUserSessionsDto : Paging
{
    public int? UserId { get; set; }

    public Guid? DeviceId { get; set; }
}