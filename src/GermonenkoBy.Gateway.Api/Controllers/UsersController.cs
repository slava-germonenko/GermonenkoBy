using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

using GermonenkoBy.Common.Web;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Gateway.Api.Contracts.Clients;
using GermonenkoBy.Gateway.Api.Models.Users;

namespace GermonenkoBy.Gateway.Api.Controllers;

[ApiController, /* Authorize, */ Route("api/users")]
public class UsersController : ControllerBaseWrapper
{
    private readonly IUsersClient _usersClient;

    private readonly IUserTerminationClient _userTerminationClient;

    public UsersController(
        IUsersClient usersClient,
        IUserTerminationClient userTerminationClient
    )
    {
        _usersClient = usersClient;
        _userTerminationClient = userTerminationClient;
    }

    [HttpGet("")]
    [SwaggerOperation("Search users.", "Search for users using filters provided via query params.")]
    [SwaggerResponse(200, "Set of users", typeof(ContentListResponse<User>))]
    public async Task<ActionResult<ContentListResponse<User>>> GetUsersAsync(
        [FromQuery, SwaggerParameter("Users filter.")] UsersFilterDto usersFilter
    )
    {
        var users = await _usersClient.GetUsersAsync(usersFilter);
        return OkWrapped(users);
    }

    [HttpGet("{userId:int}")]
    [SwaggerOperation("Get user.", "Tries to get uer by the given ID.")]
    [SwaggerResponse(200, "Found user.", typeof(ContentResponse<User>))]
    [SwaggerResponse(404, "Not found error.", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<User>>> GetUserAsync(
        [SwaggerParameter("ID of a user to search for.")] int userId
    )
    {
        var user = await _usersClient.GetUserAsync(userId);
        return user is null
            ? NotFound(new BaseResponse("Пользователь не найден."))
            : OkWrapped(user);
    }

    [HttpPost("")]
    [SwaggerOperation("Create user.", "Create user using data provided in the request body.")]
    [SwaggerResponse(200, "Created user model.", typeof(ContentResponse<User>))]
    [SwaggerResponse(400, "Bad request error.", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<User>>> CreateUserAsync(
        [FromBody, SwaggerRequestBody("User data.")] CreateUserDto userDto
    )
    {
        var user = await _usersClient.CreateUserAsync(userDto);
        return OkWrapped(user);
    }

    [HttpPatch("{userId:int}")]
    [SwaggerResponse(200, "Updated user model.", typeof(ContentResponse<User>))]
    [SwaggerResponse(400, "Bad request error.", typeof(BaseResponse))]
    [SwaggerResponse(404, "User not found error.", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<User>>> UpdateUserAsync(
        [SwaggerParameter("ID of a user to be updated")] int userId,
        [FromBody, SwaggerRequestBody("User data")] ModifyUserDto userDto
    )
    {
        var user = await _usersClient.UpdateUserAsync(userId, userDto);
        return OkWrapped(user);
    }

    [HttpDelete("{userId:int}")]
    [SwaggerResponse(204, "No content success response")]
    public async Task<NoContentResult> DeleteUserAsync(
        [SwaggerParameter("ID of a user to be deleted.")] int userId
    )
    {
        await _userTerminationClient.TerminateAsync(userId);
        return NoContent();
    }

    [HttpPatch("{userId:int}/password")]
    [SwaggerOperation("Set user's password.", "Sets new password to a user.")]
    [SwaggerResponse(204, "Success message")]
    [SwaggerResponse(400, "Password does not meet security requirements error.", typeof(BaseResponse))]
    [SwaggerResponse(404, "User not found error.", typeof(ContentResponse<BaseResponse>))]
    public async Task<NoContentResult> SetPasswordAsync(
        [SwaggerParameter("ID of a user.")] int userId,
        [FromBody, SwaggerRequestBody("New password.")] PasswordRequest passwordRequest
    )
    {
        await _usersClient.SetUserPasswordAsync(userId, passwordRequest.Password);
        return NoContent();
    }
}