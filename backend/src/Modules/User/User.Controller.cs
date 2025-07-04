/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

using Attributes;
using Core.Models;
using Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Modules.User;

[ApiJsonController]
[Authorize(Policy = Role.User)]
[Route("users")]
public class UserController(UserService userService) : Controller
{
    private readonly UserService _userService = userService;

    [HttpGet("me")]
    [ProducesResponseType(typeof(User), 200)]
    [ProducesResponseType(typeof(ResponseError), 401)]
    [ProducesResponseType(typeof(ResponseError), 403)]
    [ProducesResponseType(typeof(ResponseError), 500)]
    public User? Me()
    {
        return _userService.FindOneById(User.GetId());
    }
}
