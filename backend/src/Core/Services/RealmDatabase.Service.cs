/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

using Attributes;
using Core.Models;
using Microsoft.Extensions.Options;
using Realms;

namespace Core.Services;

[Injectable(ServiceLifetime.Transient)]
public class RealmDatabaseService
{
    private static readonly ulong CurrentSchemaVersion = 2;

    private readonly Realm _realm;
    private Transaction? _transaction;
    private readonly ILogger<RealmDatabaseService> _logger;
    private readonly IHostEnvironment _env;

    // ---

    public RealmDatabaseService(
        IOptions<AppSettings> settings,
        ILogger<RealmDatabaseService> logger,
        IHostEnvironment hostEnvironment
    )
    {
        _logger = logger;
        _env = hostEnvironment;
        _realm = Realm.GetInstance(GetConfig(settings.Value));
    }

    // ---

    private RealmConfiguration GetConfig(AppSettings settings)
    {
        string? dir;

        if (_env.IsDevelopment())
            dir = System.IO.Path.Combine(AppContext.BaseDirectory, "data");
        else
            dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        var path = System.IO.Path.Combine(dir, "app.realm");

        _logger.LogDebug("Realm database path: {0}", path);

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        return new RealmConfiguration(path)
        {
            SchemaVersion = CurrentSchemaVersion,
            MigrationCallback = MigrationCallback,
            ShouldCompactOnLaunch = ShouldCompactOnLaunch,
        };
    }

    private void MigrationCallback(Migration migration, ulong oldSchemaVersion)
    {
        if (oldSchemaVersion < CurrentSchemaVersion)
        {
            // use reflection to get all MigrationModule classes in the project. Call each of them with the migration object
            var migrationModules = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && type.GetInterface(nameof(IMigrationModule)) != null);

            foreach (var module in migrationModules)
            {
                var instance = (IMigrationModule)Activator.CreateInstance(module)!;
                _logger.LogDebug("Running migration module: {0}", module.Name);
                instance.HandleMigration(migration, oldSchemaVersion, CurrentSchemaVersion);
            }
        }
    }

    private bool ShouldCompactOnLaunch(ulong totalBytes, ulong usedBytes)
    {
        _logger.LogDebug("Checking if Realm database should be compacted.");
        _logger.LogDebug("    Total bytes used on disk: {0}", totalBytes);
        _logger.LogDebug("    Total bytes used used: {0}", usedBytes);

        // Compact if the database is over 100MB and less than 50% of the space is used
        const ulong threshold = 100 * 1024 * 1024; // 100 MB
        return totalBytes > threshold && usedBytes < totalBytes / 2;
    }

    // ---


    public T? Find<T>(string id) where T : RealmObject
    {
        ArgumentNullException.ThrowIfNull(id);

        return _realm.Find<T>(id);
    }

    public IQueryable<T> GetAll<T>() where T : RealmObject
    {
        return _realm.All<T>();
    }

    public T Add<T>(T item) where T : RealmObject
    {
        ArgumentNullException.ThrowIfNull(item);

        if (!_realm.IsInTransaction)
            _realm.Write(() => _realm.Add(item));
        else
            _realm.Add(item);

        return item;
    }

    public void Update<T>(T item) where T : RealmObject
    {
        ArgumentNullException.ThrowIfNull(item);

        if (!_realm.IsInTransaction)
            _realm.Write(() => _realm.Add(item, update: true));
        else
            _realm.Add(item, update: true);
    }

    public void Delete<T>(T item) where T : RealmObject
    {
        ArgumentNullException.ThrowIfNull(item);

        if (!_realm.IsInTransaction)
            _realm.Write(() => _realm.Remove(item));
        else
            _realm.Remove(item);
    }

    public void DeleteAll<T>() where T : RealmObject
    {
        if (!_realm.IsInTransaction)
            _realm.Write(_realm.RemoveAll<T>);
        else
            _realm.RemoveAll<T>();
    }

    public void StartTransaction()
    {
        if (!_realm.IsInTransaction)
            _transaction = _realm.BeginWrite();
    }

    public void CommitTransaction()
    {
        if (_transaction != null && _realm.IsInTransaction)
        {
            _transaction.Commit();
            _transaction = null;
        }
    }
}