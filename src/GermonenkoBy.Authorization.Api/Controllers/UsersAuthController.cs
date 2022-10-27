using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using GermonenkoBy.Authorization.Core.Dtos;
using GermonenkoBy.Authorization.Core.Models;
using GermonenkoBy.Authorization.Core.Services;
using GermonenkoBy.Common.Web;
using GermonenkoBy.Common.Web.Responses;

namespace GermonenkoBy.Authorization.Api.Controllers;

[ApiController, Route("api/users-auth")]
public class AuthController : ControllerBaseWrapper
{
    private readonly UserAuthorizationService _authorizationService;

    public AuthController(UserAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HttpPost("")]
    [SwaggerOperation("Authorizes user by login and password.")]
    [SwaggerResponse(200, "Refresh Token.", typeof(ContentResponse<RefreshToken>))]
    [SwaggerResponse(400, "Invalid login/password error", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<RefreshToken>>> AuthorizeAsync(
        [FromBody, SwaggerParameter("Authorization Request")] AuthorizeDto authorizeDto
    )
    {
        var refreshToken = await _authorizationService.AuthorizeAsync(authorizeDto);
        return OkWrapped(refreshToken);
    }
}