using System.ComponentModel.DataAnnotations;

using AutoMapper;
using Grpc.Core;

using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Users.Core.Dtos;
using GermonenkoBy.Users.Core.Exceptions;

namespace GermonenkoBy.Users.Api.Grpc;

public class UsersGrpcService : UsersService.UsersServiceBase
{
    private readonly IMapper _mapper;

    private readonly Core.UsersService _usersService;

    public UsersGrpcService(IMapper mapper, Core.UsersService usersService)
    {
        _mapper = mapper;
        _usersService = usersService;
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

    private void EnsureModelIsValid<TModel>(TModel model)
    {
        if (model != null)
        {
            Validator.ValidateObject(model, new ValidationContext(model));
        }
    }
}