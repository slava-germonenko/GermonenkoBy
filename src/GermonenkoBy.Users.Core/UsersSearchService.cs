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

        if (filter.FirstName is not null)
        {
            query = query.Where(user => user.FirstName == filter.FirstName);
        }

        if (filter.LastName is not null)
        {
            query = query.Where(user => user.LastName == filter.LastName);
        }

        if (filter.EmailAddress is not null)
        {
            query = query.Where(user => user.EmailAddress == filter.EmailAddress);
        }

        var search = filter.Search;
        if (search is not null)
        {
            query = query.Where(
                user => user.FirstName.Contains(search)
                        || user.LastName.Contains(search)
                        || user.EmailAddress.Contains(search)
            );
        }

        switch (filter.OrderBy?.ToLower())
        {
            case "firstname":
            {
                query = filter.OrderDirection.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderByDescending(user => user.FirstName)
                    : query.OrderBy(user => user.FirstName);
                break;
            }
            case "lastname":
            {
                query = filter.OrderDirection.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderByDescending(user => user.LastName)
                    : query.OrderBy(user => user.LastName);
                break;
            }
            case "emailaddress":
            {
                query = filter.OrderDirection.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderByDescending(user => user.EmailAddress)
                    : query.OrderBy(user => user.EmailAddress);
                break;
            }
            default:
            {
                query = query.OrderByDescending(user => user.CreatedDate);
                break;
            }
        }

        return await query.ToPagedSet(filter);
    }
}