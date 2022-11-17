using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.EntityFramework.Extensions;
using GermonenkoBy.Users.Core.Dtos;
using GermonenkoBy.Users.Core.Models;

namespace GermonenkoBy.Users.Core;

public class UsersSearchService
{
    private readonly UsersContext _context;

    public UsersSearchService(UsersContext context)
    {
        _context = context;
    }

    public async Task<PagedSet<User>> SearchUsersListAsync(UsersFilterDto filter)
    {
        var query = _context.Users.AsNoTracking();

        if (filter.Active is not null)
        {
            query = query.Where(user => user.Active == filter.Active);
        }

        if (!string.IsNullOrEmpty(filter.FirstName))
        {
            query = query.Where(user => user.FirstName == filter.FirstName);
        }

        if (!string.IsNullOrEmpty(filter.LastName))
        {
            query = query.Where(user => user.LastName == filter.LastName);
        }

        if (!string.IsNullOrEmpty(filter.EmailAddress))
        {
            query = query.Where(user => user.EmailAddress == filter.EmailAddress);
        }

        var search = filter.Search;
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(
                user => user.FirstName.Contains(search)
                        || user.LastName.Contains(search)
                        || user.EmailAddress.Contains(search)
            );
        }

        var descendingOrder = filter.OrderBy is not null
                              && filter.OrderDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

        Expression<Func<User, object>> orderKeySelector = filter.OrderBy?.ToLower() switch
        {
            "firstname" => user => user.FirstName,
            "lastname" => user => user.LastName,
            "emailaddress" => user => user.EmailAddress,
            "updateddate" => user => user.UpdatedDate!,
            _ => user => user.CreatedDate
        };

        query = descendingOrder
            ? query.OrderByDescending(orderKeySelector)
            : query.OrderBy(orderKeySelector);

        return await query.ToPagedSetAsync(filter);
    }
}