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

    private const string AccessTokenCookieName = "germonenko.by-access";

    private const string RefreshTokenCookieName = "germonenko.by-refresh";

    public AuthorizationController(UserAuthService userAuthService)
    {
        _userAuthService = userAuthService;
    }

    [HttpPost(""), Produces("application/json"),]
    [SwaggerOperation("User auth.", "Authorizes user by login and password.")]
    [SwaggerResponse(200, "Access token, refresh token and user data.", typeof(ContentResponse<AuthorizationResult>))]
    [SwaggerResponse(400, "Invalid login/password error.")]
    public async Task<ActionResult<ContentResponse<AuthorizationResult>>> AuthorizeAsync(
        [FromBody, SwaggerRequestBody("User authorization data.")] AuthorizeDto authorizeDto
    )
    {
        var authResult = await _userAuthService.AuthorizeAsync(authorizeDto);
        SetAccessTokenCookie(authResult.AccessToken);
        SetRefreshTokenCookie(authResult.RefreshToken);
        return OkWrapped(authResult);
    }

    [HttpPost("refresh"), Produces("application/json"),]
    [SwaggerOperation("Refresh token.", "Refreshes access and refresh tokens.")]
    [SwaggerResponse(200, "Refresh (auth) result.", typeof(ContentResponse<AuthorizationResult>))]
    [SwaggerResponse(401, "Unauthorized error.", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<AuthorizationResult>>> AuthorizeAsync()
    {
        if (!Request.Cookies.TryGetValue(RefreshTokenCookieName, out var token) || token is null)
        {
            return Unauthorized(new BaseResponse("Ошибка доступа (refresh token не был найден в cookies.)"));
        }

        var refreshResult = await _userAuthService.RefreshAsync(token);
        SetAccessTokenCookie(refreshResult.AccessToken);
        SetRefreshTokenCookie(refreshResult.RefreshToken);
        return OkWrapped(refreshResult);
    }

    [HttpPost("terminate")]
    [SwaggerOperation("Terminate session.", "Removes refresh token and corresponding session")]
    public async Task<NoContentResult> TerminateAsync()
    {
        if (Request.Cookies.TryGetValue(RefreshTokenCookieName, out var token) && token is not null)
        {
            await _userAuthService.TerminateAsync(token);
        }

        return NoContent();
    }

    private void SetRefreshTokenCookie(RefreshToken refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            Domain = "",
            Expires = refreshToken.ExpireDate,
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
        };
        Response.Cookies.Append(RefreshTokenCookieName, refreshToken.Token, cookieOptions);
    }

    private void SetAccessTokenCookie(AccessToken accessToken)
    {
        var cookieOptions = new CookieOptions
        {
            Domain = "",
            Expires = accessToken.ExpireDate,
            SameSite = SameSiteMode.Strict,
        };
        Response.Cookies.Append(AccessTokenCookieName, accessToken.Token, cookieOptions);
    }
}