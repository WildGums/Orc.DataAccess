namespace Orc.DataAccess.Database;

using System;

public class DbDataSource
{
    public DbDataSource(string providerInvariantName, string instanceName)
    {
        ArgumentNullException.ThrowIfNull(providerInvariantName);
        ArgumentNullException.ThrowIfNull(instanceName);

        ProviderInvariantName = providerInvariantName;
        InstanceName = instanceName;
    }
    public string ProviderInvariantName { get; }
    public string InstanceName { get; }

    protected bool Equals(DbDataSource other)
    {
        return string.Equals(InstanceName, other.InstanceName);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == GetType() && Equals((DbDataSource)obj);
    }

    public override int GetHashCode()
    {
        return InstanceName.GetHashCode();
    }
}
