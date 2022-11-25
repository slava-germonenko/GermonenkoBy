using System.ComponentModel.DataAnnotations;

using AutoMapper;
using Grpc.Core;

using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Users.Core;
using GermonenkoBy.Users.Core.Dtos;
using GermonenkoBy.Users.Core.Exceptions;

namespace GermonenkoBy.Users.Api.Grpc;

public class UsersGrpcService : UsersService.UsersServiceBase
{
    private readonly IMapper _mapper;

    private readonly PasswordValidationService _passwordValidationService;

    private readonly Core.UsersService _usersService;

    private readonly UsersSearchService _usersSearchService;

    public UsersGrpcService(
        IMapper mapper,
        PasswordValidationService passwordValidationService,
        Core.UsersService usersService,
        UsersSearchService usersSearchService
    )
    {
        _mapper = mapper;
        _passwordValidationService = passwordValidationService;
        _usersService = usersService;
        _usersSearchService = usersSearchService;
    }

    public override async Task<UserResponse> GetUser(GetSingleUserRequest request, ServerCallContext context)
    {
        try
        {
            var user = await _usersService.GetUserAsync(request.UserId);
            return _mapper.Map<UserResponse>(user);
        }
        catch (NotFoundException e)
        {
            context.Status = new Status(StatusCode.NotFound, e.Message);
            return new UserResponse();
        }
    }

    public override async Task<UsersListResponse> SearchUsers(SearchUsersRequest request, ServerCallContext context)
    {
        var filter = _mapper.Map<UsersFilterDto>(request);
        var usersList = await _usersSearchService.SearchUsersListAsync(filter);
        var response = new UsersListResponse
        {
            Count = usersList.Count,
            Total = usersList.Total,
            Offset = usersList.Offset,
        };
        response.Users.AddRange(
            usersList.Data.Select(_mapper.Map<UserResponse>)
        );
        return response;
    }

    public override async Task<UserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        try
        {
            var userDto = _mapper.Map<CreateUserDto>(request);
            EnsureModelIsValid(userDto);
            var createdUser = await _usersService.CreateUserAsync(userDto);
            return _mapper.Map<UserResponse>(createdUser);
        }
        catch (EmailAddressInUseException e)
        {
            context.Status = new Status(StatusCode.AlreadyExists, e.Message);
            return new UserResponse();
        }
        catch (Exception e) when (e is ValidationException or CoreLogicException)
        {
            context.Status = new Status(StatusCode.InvalidArgument, e.Message);
            return new UserResponse();
        }
    }

    public override async Task<UserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        try
        {
            var updateDto = _mapper.Map<ModifyUserDto>(request);
            EnsureModelIsValid(updateDto);
            var updatedUser = await _usersService.UpdateUserBasicDataAsync(request.UserId, updateDto);
            return _mapper.Map<UserResponse>(updatedUser);
        }
        catch (EmailAddressInUseException e)
        {
            context.Status = new Status(StatusCode.AlreadyExists, e.Message);
            return new UserResponse();
        }
        catch (NotFoundException e)
        {
            context.Status = new Status(StatusCode.NotFound, e.Message);
            return new UserResponse();
        }
        catch (Exception e) when (e is ValidationException or CoreLogicException)
        {
            context.Status = new Status(StatusCode.InvalidArgument, e.Message);
            return new UserResponse();
        }
    }

    public override async Task<UserResponse> UpdateUserPassword(UpdateUserPasswordRequest request, ServerCallContext context)
    {
        try
        {
            var user = await _usersService.UpdateUserPasswordAsync(request.UserId, request.Password);
            return _mapper.Map<UserResponse>(user);
        }
        catch (NotFoundException e)
        {
            context.Status = new Status(StatusCode.NotFound, e.Message);
            return new UserResponse();
        }
        catch (Exception e) when (e is ValidationException or CoreLogicException)
        {
            context.Status = new Status(StatusCode.InvalidArgument, e.Message);
            return new UserResponse();
        }
    }

    public override async Task<PasswordValidationResponse> ValidatePassword(
        ValidatePasswordRequest request,
        ServerCallContext context
    )
    {
        try
        {
            var valid = await _passwordValidationService.PasswordIsValidAsync(request.UserId, request.Password);
            return new PasswordValidationResponse
            {
                Status = valid ? UserPasswordStatus.Valid : UserPasswordStatus.Invalid,
            };
        }
        catch (NotFoundException)
        {
            return new PasswordValidationResponse
            {
                Status = UserPasswordStatus.UserNotFound
            };
        }
    }

    public override async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        var existed = false;
        try
        {
            var user = await _usersService.GetUserAsync(request.UserId);
            await _usersService.RemoveUserAsync(user.Id);
            existed = true;
        }
        catch (NotFoundException) { }

        return new DeleteUserResponse
        {
            Existed = existed,
        };
    }

    private void EnsureModelIsValid<TModel>(TModel model)
    {
        if (model != null)
        {
            Validator.ValidateObject(model, new ValidationContext(model));
        }
    }
}