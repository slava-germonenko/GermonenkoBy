using AutoMapper;
using Grpc.Core;

using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Gateway.Api.Models.Users;
using GermonenkoBy.Users.Api.Grpc;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients.Grpc;

public class GrpcUsersClient : IUsersClient
{
    private readonly IMapper _mapper;

    private readonly UsersService.UsersServiceClient _usersGrpcClient;

    public GrpcUsersClient(IMapper mapper, UsersService.UsersServiceClient usersGrpcClient)
    {
        _mapper = mapper;
        _usersGrpcClient = usersGrpcClient;
    }

    public async Task<User?> GetUserAsync(int userId)
    {
        try
        {
            var user = await _usersGrpcClient.GetUserAsync(new()
            {
                UserId = userId,
            });
            return _mapper.Map<User>(user);
        }
        catch (RpcException e) when (e.StatusCode == StatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<PagedSet<User>> GetUsersAsync(UsersFilterDto usersFilter)
    {
        var searchRequest = _mapper.Map<SearchUsersRequest>(usersFilter);
        var searchResponse = await _usersGrpcClient.SearchUsersAsync(searchRequest);
        return new()
        {
            Total = searchResponse.Total,
            Count = searchResponse.Count,
            Offset = searchResponse.Offset,
            Data = searchResponse.Users.Select(_mapper.Map<User>).ToList()
        };
    }

    public async Task<User> CreateUserAsync(CreateUserDto userDto)
    {
        try
        {
            var createUserRequest = _mapper.Map<CreateUserRequest>(userDto);
            var response = await _usersGrpcClient.CreateUserAsync(createUserRequest);
            return _mapper.Map<User>(response);
        }
        catch (RpcException e) when (e.StatusCode is StatusCode.InvalidArgument or StatusCode.AlreadyExists)
        {
            throw new CoreLogicException(e.Message);
        }
    }

    public async Task<User> UpdateUserAsync(int userId, ModifyUserDto userDto)
    {
        try
        {
            var updateUserRequest = _mapper.Map<UpdateUserRequest>(userDto);
            updateUserRequest.UserId = userId;
            var response = await _usersGrpcClient.UpdateUserAsync(updateUserRequest);
            return _mapper.Map<User>(response);
        }
        catch (RpcException e) when (e.StatusCode is StatusCode.InvalidArgument or StatusCode.AlreadyExists)
        {
            throw new CoreLogicException(e.Message);
        }
        catch (RpcException e) when (e.StatusCode is StatusCode.NotFound)
        {
            throw new NotFoundException(e.Message);
        }
    }

    public async Task SetUserPasswordAsync(int userId, string password)
    {
        try
        {
            await _usersGrpcClient.UpdateUserPasswordAsync(new()
            {
                UserId = userId,
                Password = password,
            });
        }
        catch (RpcException e) when (e.StatusCode is StatusCode.InvalidArgument)
        {
            throw new CoreLogicException(e.Message);
        }
        catch (RpcException e) when (e.StatusCode is StatusCode.NotFound)
        {
            throw new NotFoundException(e.Message);
        }
    }

    public async Task DeleteUserAsync(int userId)
    {
        await _usersGrpcClient.DeleteUserAsync(new()
        {
            UserId = userId,
        });
    }
}