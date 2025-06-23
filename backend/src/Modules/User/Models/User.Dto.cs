/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

namespace Modules.User.Models;

#region Models

public class UserDto
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Password { get; set; }
    public required bool IsAdmin { get; set; }
}

#endregion
