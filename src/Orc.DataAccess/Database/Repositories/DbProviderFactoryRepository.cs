namespace Orc.DataAccess.Database;

using System;
using System.Configuration;
using System.Data;
using System.Linq;
using Catel.Logging;

public class DbProviderFactoryRepository
{
    /// <summary>
    /// Name of the configuration element.
    /// </summary>
    private const string DbProviderFactoriesElement = "DbProviderFactories";

    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private DataTable? _dbProviderFactoryTable;

    /// <summary>
    /// Adds the specified provider.
    /// </summary>
    /// <param name="providerInfo">The provider.</param>
    public void Add(DbProviderInfo providerInfo)
    {
        ArgumentNullException.ThrowIfNull(providerInfo);

        var providerTable = GetProviderTable();
        if (providerTable is null)
        {
            return;
        }

        Remove(providerInfo);
        providerTable.Rows.Add(providerInfo.Name, providerInfo.Description, providerInfo.InvariantName, providerInfo.AssemblyQualifiedName);
    }

    /// <summary>
    /// Deletes the specified provider if present.
    /// </summary>
    /// <param name="providerInfo">The provider.</param>
    public void Remove(DbProviderInfo providerInfo)
    {
        ArgumentNullException.ThrowIfNull(providerInfo);

        var providerTable = GetProviderTable();
        if (providerTable is null)
        {
            return;
        }

        var row = providerTable.Rows.Cast<DataRow>()
            .FirstOrDefault(o => o[2].ToString() == providerInfo.InvariantName);

        if (row is not null)
        {
            providerTable.Rows.Remove(row);
        }
    }

    private DataTable? GetProviderTable()
    {
        if (_dbProviderFactoryTable is not null)
        {
            return _dbProviderFactoryTable;
        }

        // Open the configuration.
        if (ConfigurationManager.GetSection("system.data") is not DataSet dataConfiguration)
        {
            Log.Error("Unable to open 'System.Data' from the configuration");

            return null;
        }

        // Open the provider table.
        if (!dataConfiguration.Tables.Contains(DbProviderFactoriesElement))
        {
            Log.Error($"Unable to open the '{DbProviderFactoriesElement}' table");

            return null;
        }

        _dbProviderFactoryTable = dataConfiguration.Tables[DbProviderFactoriesElement];

        return _dbProviderFactoryTable;
    }
}
