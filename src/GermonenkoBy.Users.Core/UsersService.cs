using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Common.EntityFramework.Extensions;
using GermonenkoBy.Common.Utils.Hashing;
using GermonenkoBy.Users.Core.Contracts;
using GermonenkoBy.Users.Core.Dtos;
using GermonenkoBy.Users.Core.Extensions;
using GermonenkoBy.Users.Core.Models;

namespace GermonenkoBy.Users.Core;

public class UsersService
{
    private readonly IHasher _hasher;

    private readonly IPasswordPolicy _passwordPolicy;

    private readonly UsersContext _context;

    public UsersService(IHasher hasher, IPasswordPolicy passwordPolicy, UsersContext context)
    {
        _context = context;
        _hasher = hasher;
        _passwordPolicy = passwordPolicy;
    }

    public async Task<User> GetUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null)
        {
            throw new NotFoundException("Пользователь не найден.");
        }

        return user;
    }

    public async Task<User> CreateUserAsync(CreateUserDto userDto)
    {
        EnsureUserIdValid(userDto);

        if (!_passwordPolicy.PasswordMeetsPolicyRequirements(userDto.Password))
        {
            throw new CoreLogicException(_passwordPolicy.PolicyDescription);
        }

        var emailAddressIsInUse = _context.Users.Any(u => u.EmailAddress == userDto.EmailAddress);
        if (emailAddressIsInUse)
        {
            var message = $"Адрес электронной почты \"{userDto.EmailAddress}\" уже использутеся другим пользователем.";
            throw new CoreLogicException(message);
        }

        var user = new User();
        user.CopyDetailsFrom(userDto);

        (user.PasswordHash, user.PasswordHash) = _hasher.GetHash(userDto.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> UpdateUserBasicDataAsync(int userId, ModifyUserDto userDto)
    {
        EnsureUserIdValid(userDto);

        var emailAddressIsInUse = _context.Users.Any(u => u.EmailAddress == userDto.EmailAddress && u.Id != userId);
        if (emailAddressIsInUse)
        {
            var message = $"Адрес электронной почты \"{userDto.EmailAddress}\" уже использутеся другим пользователем.";
            throw new CoreLogicException(message);
        }

        var user = await GetUserAsync(userId);
        user.CopyDetailsFrom(userDto);

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> UpdateUserPasswordAsync(int userId, string newPassword)
    {
        if (!_passwordPolicy.PasswordMeetsPolicyRequirements(newPassword))
        {
            throw new CoreLogicException(_passwordPolicy.PolicyDescription);
        }

        var user = await GetUserAsync(userId);
        (user.PasswordHash, user.PasswordSalt) = _hasher.GetHash(newPassword);

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task RemoveUserAsync(int userId)
    {
        var userToRemove = await GetUserAsync(userId);
        _context.Users.Remove(userToRemove);
        await _context.SaveChangesAsync();
    }

    private void EnsureUserIdValid(ModifyUserDto userDto)
    {
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(userDto, new ValidationContext(userDto), validationResults, true);
        if (validationResults.Any()) {
            throw new CoreLogicException(validationResults[0].ErrorMessage ?? "Данные пользователя некорректны!");
        }
    }
}