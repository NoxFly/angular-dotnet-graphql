/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

using Attributes;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Modules.Auth;

[ApiJsonController]
[Route("auth")]
public class AuthController(AuthService authService) : Controller
{
    private readonly AuthService _authService = authService;

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(typeof(ResponseError), 400)]
    [ProducesResponseType(typeof(ResponseError), 500)]
    [ProducesResponseType(typeof(ResponseError), 503)]
    public AuthResponse Login([FromBody] CredentialsBody credentials)
    {
        return _authService.Login(credentials, HttpContext);
    }

    [HttpPost("verify")]
    [Authorize(Policy = Role.User)]
    [ProducesResponseType(typeof(NoContentResult), 204)]
    [ProducesResponseType(typeof(ResponseError), 401)]
    public NoContentResult Verify()
    {
        return NoContent();
    }
}
