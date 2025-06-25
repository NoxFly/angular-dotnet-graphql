/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

using Realms;

namespace Modules.User;

[MapTo("User")]
public class UserSchema : RealmObject
{
    [PrimaryKey]
    public string Id { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Password { get; set; } = default!;

    public bool IsAdmin { get; set; }
}