/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

using Realms;

namespace Core.Models;

public abstract class IMigrationModule
{
    public abstract void HandleMigration(Migration migration, ulong oldSchemaVersion, ulong newSchemaVersion);
}