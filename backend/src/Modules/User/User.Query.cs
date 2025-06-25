/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

using Modules.User;

namespace Core.Models;

public partial class QueryType
{
    public IQueryable<User> Users([Service] UserService userService) => userService.GetUsers();
    public User? GetUser([Service] UserService userService, [ID] string id) => userService.FindOneById(id);
}
