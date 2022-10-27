using GermonenkoBy.Authorization.Core.Contracts;

namespace GermonenkoBy.Authorization.Infrastructure.Contracts;

public class EndOfNextDayExpireDateGenerator : IExpireDateGenerator
{
    public DateTime GenerateSessionExpireDate()
    {
        var nextDay = DateTime.UtcNow.AddDays(1);
        return new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, 23, 59, 59);
    }
}