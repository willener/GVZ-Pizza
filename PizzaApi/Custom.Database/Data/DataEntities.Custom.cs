using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Custom.Database.Data;

/// <summary>
/// Partial extension class for entity framework data context.
/// </summary>
public partial class DataEntities {

    /// <summary>
    /// The default database server.
    /// </summary>
    public const string DEFAULT_DATABASE_ROOT = ".";

    /// <summary>
    /// The default database name.
    /// </summary>
    public const string DEFAULT_DATABASE_NAME = "PizzaShop";

    /// <summary>
    /// The default database user.
    /// </summary>
    public const string DEFAULT_DATABASE_USER = "test";

    /// <summary>
    /// The default database password.
    /// </summary>
    public const string DEFAULT_DATABASE_PASSWORD = "test";

    /// <summary>
    /// The default database security active.
    /// </summary>
    public const bool DEFAULT_DATABASE_INT_SECURITY = true;

    /// <summary>
    /// Default database connect timeout in seconds.
    /// </summary>
    public const int DEFAULT_CONNECT_TIMEOUT_SEC = 15;

    /// <summary>
    /// Sets the database connection string.
    /// </summary>
    public static void SetDefaultSettings() {        
        s_defaultConnectionString = BuildConnectionString(
            DEFAULT_DATABASE_ROOT,
            DEFAULT_DATABASE_NAME,
            DEFAULT_DATABASE_INT_SECURITY,
            DEFAULT_DATABASE_USER,
            DEFAULT_DATABASE_PASSWORD,
            DEFAULT_CONNECT_TIMEOUT_SEC);
    }

    /// <summary>
    /// Builds a connection string.
    /// </summary>
    /// <param name="serverName">Server name, e.g. "."</param>
    /// <param name="databaseName">Database name, e.g. "AdventureWorks"</param>
    /// <param name="integratedSecurity"></param>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="connectTimeout"></param>
    /// <returns></returns>
    private static string BuildConnectionString(string? serverName, string? databaseName, bool integratedSecurity, string? username, string? password, int? connectTimeout = null) {
        // Build the SqlClient connection string.
        var sqlConnectionStringBuilder = new SqlConnectionStringBuilder() {
            DataSource = string.IsNullOrEmpty(serverName) ? "." : serverName
        };
        if (!string.IsNullOrEmpty(databaseName)) {
            sqlConnectionStringBuilder.InitialCatalog = databaseName;
        }
        sqlConnectionStringBuilder.IntegratedSecurity = integratedSecurity;
        sqlConnectionStringBuilder.UserID = username ?? string.Empty;
        sqlConnectionStringBuilder.Password = password ?? string.Empty;
        sqlConnectionStringBuilder.ConnectTimeout = connectTimeout ?? DEFAULT_CONNECT_TIMEOUT_SEC;
        sqlConnectionStringBuilder.Encrypt = true; // Since EF 7.0 the default of Encrypt is true
        sqlConnectionStringBuilder.TrustServerCertificate = true; // TODO: Since EF 7.0 the default of Encrypt is true. Set TrustServerCertificate to false in exposed environments. Certificates needs to be trusted in this case (not self signed).
        return sqlConnectionStringBuilder.ToString();
    }

    /// <summary>
    /// Gets a new instance of the entity framework object context.
    /// </summary>
    /// <param name="connectTimeout"></param>
    /// <param name="commandTimeout"></param>
    /// <returns>Entity Framework Object Context</returns>
    public static DataEntities GetNewInstance(int? connectTimeout = null, int? commandTimeout = null) {
        string? connectionString = null;
        if (string.IsNullOrWhiteSpace(s_defaultConnectionString) && connectTimeout == null) {
            SetDefaultSettings();
        }

        // Use a new connection string with custom connect timeout.
        connectionString = s_defaultConnectionString;
        // Use the connection string from custom config file
        var builder = new DbContextOptionsBuilder<DataEntities>();
        if (commandTimeout != null) {
            builder = builder.UseSqlServer(connectionString, opts => opts.CommandTimeout((int)TimeSpan.FromSeconds(commandTimeout.Value).TotalSeconds));
        }
        else {
            builder = builder.UseSqlServer(connectionString);
        }
        return new DataEntities(builder.Options);
    }

    /// <summary>
    /// Persists all updates to the data source and resets change tracking in the object context.
    /// </summary>
    /// <returns>True on success</returns>
    public new bool SaveChanges() {
        bool success = false;
        try {
            // Try to save changes, which may cause a conflict.
            base.SaveChanges();
            return true;
        }
        catch (Exception ex) {
            string message = "Exception during Entity Framework SaveChanges()\nException: " + ex.Message;
            if (ex is DbUpdateException dbUpdateEx) {
                foreach (var entry in dbUpdateEx.Entries) {
                    message += ("\nDbUpdateException for Entity :" + entry.Entity.ToString());
                    message += ("\nDebug Message Short: " + entry.DebugView.ShortView);
                    message += ("\nDebug Message Long:: " + entry.DebugView.LongView);
                }
            }
        }
        return success;
    }

    private static string? s_defaultConnectionString;
}
