using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.Web;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Users.Api.Models;
using GermonenkoBy.Users.Core;
using GermonenkoBy.Users.Core.Dtos;

namespace GermonenkoBy.Users.Api.Controllers;

[ApiController, Route("api/users")]
public class UsersController : ControllerBaseWrapper
{
    private readonly UsersService _usersService;

    private readonly UsersSearchService _usersSearchService;

    public UsersController(UsersService usersService, UsersSearchService usersSearchService)
    {
        _usersService = usersService;
        _usersSearchService = usersSearchService;
    }

    [HttpGet("")]
    [SwaggerOperation("Search users.", "Search for users using filters provided via query params.")]
    [SwaggerResponse(200, "List of Users.", typeof(ContentListResponse<UserViewModel>))]
    public async Task<ActionResult<ContentListResponse<UserViewModel>>> GetUsersAsync(
        [FromQuery, SwaggerParameter("Users Filter")] UsersFilterDto filter
    )
    {
        var usersSet = await _usersSearchService.SearchUsersListAsync(filter);
        var userViewModels = usersSet.Data.Select(UserViewModel.FromUser).ToList();
        var userViewModelsSet = new PagedSet<UserViewModel>
        {
            Count = usersSet.Count,
            Offset = usersSet.Offset,
            Total = usersSet.Total,
            Data = userViewModels,
        };
        return OkWrapped(userViewModelsSet);
    }

    [HttpGet("{userId:int}")]
    [SwaggerOperation("Get user.", "Tries to get uer by the given ID.")]
    [SwaggerResponse(200, "Instance the user.", typeof(ContentResponse<UserViewModel>), "application/json")]
    [SwaggerResponse(404, "Not found error.", typeof(BaseResponse), "application/json")]
    public async Task<ActionResult<ContentResponse<UserViewModel>>> GetUserAsync(
        [SwaggerParameter("User ID to search for.")] int userId
    )
    {
        var user = await _usersService.GetUserAsync(userId);
        return OkWrapped(UserViewModel.FromUser(user));
    }

    [HttpPost("")]
    [SwaggerOperation("Create user.", "Creates new product with the data provided in the body.")]
    [SwaggerResponse(200, "Instance of the updated user.", typeof(ContentResponse<UserViewModel>), "application/json")]
    [SwaggerResponse(400, "Bad request error.", typeof(BaseResponse), "application/json")]
    public async Task<ActionResult<ContentResponse<UserViewModel>>> CreateUserAsync(
        [FromBody, SwaggerRequestBody("User model to add.")] CreateUserDto userDto
    )
    {
        var user = await _usersService.CreateUserAsync(userDto);
        return OkWrapped(UserViewModel.FromUser(user));
    }

    [HttpPatch("{userId:int}")]
    [SwaggerOperation("Update user.", "Updates user with the data supplied in the request body.")]
    [SwaggerResponse(200, "Instance of the updated user.", typeof(ContentResponse<UserViewModel>), "application/json")]
    [SwaggerResponse(400, "Bad request error.", typeof(BaseResponse), "application/json")]
    [SwaggerResponse(404, "Not found error.", typeof(BaseResponse), "application/json")]
    public async Task<ActionResult<ContentResponse<UserViewModel>>> UpdateUserDetailsAsync(
        [SwaggerParameter("ID of a user to be updated.")] int userId,
        [FromBody, SwaggerRequestBody("Model containing update information for the user.")] ModifyUserDto userDto
    )
    {
        var user = await _usersService.UpdateUserBasicDataAsync(userId, userDto);
        return OkWrapped(UserViewModel.FromUser(user));
    }

    [HttpDelete("{userId:int}")]
    [SwaggerOperation("Remove user.", "Hard deletes user with the given ID.")]
    [SwaggerResponse(204, "No content message that signals that user was successfully deleted.")]
    [SwaggerResponse(400, "Not found error.", typeof(BaseResponse), "application/json")]
    public async Task<NoContentResult> RemoveUserAsync(
        [SwaggerParameter("ID of a user to be removed.")] int userId
    )
    {
        await _usersService.RemoveUserAsync(userId);
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
        await _usersService.SetPasswordAsync(userId, passwordRequest.Password);
        return NoContent();
    }

    [HttpPost("{userId:int}/password-validation")]
    [SwaggerOperation(
        "Validate user password.",
        "Checks if hash of the password provided in the body matches user's password hash."
    )]
    [SwaggerResponse(204, "No Content message. If 204 is returned, then a passed password is valid")]
    [SwaggerResponse(400, "Error message. If 400 is returned then password is invalid.")]
    [SwaggerResponse(404, "User is not found.")]
    public async Task<ActionResult> ValidatePasswordAsync(
        [FromRoute, SwaggerParameter("ID of a user whose password should be validated.")] int userId,
        [FromBody, SwaggerRequestBody("Password to validate.")] PasswordRequest request,
        [FromServices] PasswordValidationService validationService
    )
    {
        var valid = await validationService.PasswordIsValidAsync(userId, request.Password);
        return valid ? NoContent() : BadRequest(new BaseResponse("Неправильный пароль."));
    }
}