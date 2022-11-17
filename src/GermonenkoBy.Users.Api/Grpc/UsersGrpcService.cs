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

    private readonly Core.UsersService _usersService;

    private readonly UsersSearchService _usersSearchService;

    public UsersGrpcService(IMapper mapper, Core.UsersService usersService, UsersSearchService usersSearchService)
    {
        _mapper = mapper;
        _usersService = usersService;
        _usersSearchService = usersSearchService;
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

    private void EnsureModelIsValid<TModel>(TModel model)
    {
        if (model != null)
        {
            Validator.ValidateObject(model, new ValidationContext(model));
        }
    }
}