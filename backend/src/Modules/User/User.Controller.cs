using Attributes;
using Core.Models;
using Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.User.Dto;

namespace Modules.User;

[ApiJsonController]
[Authorize(Policy = Role.User)]
[Route("users")]
public class UserController(UserService userService) : Controller
{
    private readonly UserService _userService = userService;

    [HttpGet("me")]
    [ProducesResponseType(typeof(UserDto), 200)]
    [ProducesResponseType(typeof(ResponseError), 401)]
    [ProducesResponseType(typeof(ResponseError), 403)]
    [ProducesResponseType(typeof(ResponseError), 500)]
    public async Task<UserDto?> Me()
    {
        return await _userService.FindOneById(User.GetId());
    }
}
