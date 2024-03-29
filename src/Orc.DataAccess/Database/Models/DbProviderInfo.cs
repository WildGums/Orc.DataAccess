﻿namespace Orc.DataAccess.Database;

using System;

public class DbProviderInfo
{
    public DbProviderInfo(string name, string invariantName, string description, string assemblyQualifiedName)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(invariantName);
        ArgumentNullException.ThrowIfNull(description);
        ArgumentNullException.ThrowIfNull(assemblyQualifiedName);

        Name = name;
        InvariantName = invariantName;
        Description = description;
        AssemblyQualifiedName = assemblyQualifiedName;
    }

    public string Name { get; }
    public string InvariantName { get; }
    public string Description { get; }
    public string AssemblyQualifiedName { get; }

    protected bool Equals(DbProviderInfo other)
    {
        return string.Equals(InvariantName, other.InvariantName);
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

        return obj.GetType() == GetType() && Equals((DbProviderInfo)obj);
    }

    public override int GetHashCode()
    {
        return InvariantName.GetHashCode();
    }
}
