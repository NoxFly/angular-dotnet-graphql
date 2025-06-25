/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

using Core.Models;
using Realms;

namespace Modules.User;

public class UserMigration1To2 : IMigrationModule
{
    public override void HandleMigration(Migration migration, ulong oldSchemaVersion, ulong newSchemaVersion)
    {
        // rename :
        // - string Name -> string FirstName
        // add :
        // - string LastName
        if (oldSchemaVersion == 1 && newSchemaVersion == 2)
        {
            migration.RenameProperty("string", "Name", "FirstName");
        }
    }
}