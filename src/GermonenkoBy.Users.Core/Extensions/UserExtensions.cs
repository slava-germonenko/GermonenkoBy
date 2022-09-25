using GermonenkoBy.Users.Core.Dtos;
using GermonenkoBy.Users.Core.Models;

namespace GermonenkoBy.Users.Core.Extensions;

public static class UserExtensions
{
    public static void CopyDetailsFrom(this User user, ModifyUserDto userDto)
    {
        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.EmailAddress = userDto.EmailAddress;
        user.Active = userDto.Active;
    }
}