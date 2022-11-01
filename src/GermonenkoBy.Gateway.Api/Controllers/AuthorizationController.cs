using Microsoft.AspNetCore.Mvc;

using GermonenkoBy.Common.Web;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Gateway.Api.Models.Auth;
using GermonenkoBy.Gateway.Api.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace GermonenkoBy.Gateway.Api.Controllers;

[ApiController, Route("api/auth")]
public class AuthorizationController : ControllerBaseWrapper
{
    private readonly UserAuthService _userAuthService;

    public AuthorizationController(UserAuthService userAuthService)
    {
        _userAuthService = userAuthService;
    }

    [HttpPost("")]
    [SwaggerOperation("User auth.", "Authorizes user by login and password.")]
    [SwaggerResponse(200, "Access token, refresh token and user data.", typeof(ContentResponse<AuthorizationResult>))]
    [SwaggerResponse(400, "Invalid login/password error.")]
    public async Task<ActionResult<ContentResponse<AuthorizationResult>>> AuthorizeAsync(
        [FromBody, SwaggerRequestBody("User authorization data.")] AuthorizeDto authorizeDto
    )
    {
        var authResult = await _userAuthService.AuthorizeAsync(authorizeDto);
        return OkWrapped(authResult);
    }
}