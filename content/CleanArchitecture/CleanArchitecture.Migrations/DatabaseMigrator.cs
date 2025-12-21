using System.Reflection;
using DbUp;

namespace CleanArchitecture.Migrations;

public class DatabaseMigrator
{
    public static void Migrate(string connectionString)
    {
        var upgrader =
            DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            throw new Exception("Database migration failed", result.Error);
        }
    }
}