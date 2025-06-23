/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

using Realms;

namespace Modules.User.Models;

[MapTo("User")]
public class UserSchema : RealmObject
{
    [PrimaryKey]
    public required string Id { get; set; }

    public required string Name { get; set; }

    public required string Password { get; set; }

    public required bool IsAdmin { get; set; }
}