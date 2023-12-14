namespace Orc.DataAccess.Database;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using Catel.Collections;
using Catel.Data;

public class DbConnectionString : ModelBase
{
    private readonly DbConnectionStringBuilder _connectionStringBuilder;

    public DbConnectionString(DbConnectionStringBuilder connectionStringBuilder, DbProviderInfo dbProvider)
    {
        ArgumentNullException.ThrowIfNull(connectionStringBuilder);
        ArgumentNullException.ThrowIfNull(dbProvider);

        _connectionStringBuilder = connectionStringBuilder;
        DbProvider = dbProvider;
        Properties = new Dictionary<string, DbConnectionStringProperty>();

        UpdateProperties();
    }

    public IReadOnlyDictionary<string, DbConnectionStringProperty> Properties { get; private set; }

    public DbProviderInfo DbProvider { get; }

    public DbConnectionStringBuilder ConnectionStringBuilder => _connectionStringBuilder;

    private void UpdateProperties()
    {
        var sensitiveProperties = TypeDescriptor.GetProperties(_connectionStringBuilder, new Attribute[]
            {
                PasswordPropertyTextAttribute.Yes
            })
            .OfType<PropertyDescriptor>()
            .Select(x => x.DisplayName);

        var sensitivePropertiesHashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        sensitivePropertiesHashSet.AddRange(sensitiveProperties);
        sensitivePropertiesHashSet.Add("Password");

        var propDescriptor = _connectionStringBuilder as ICustomTypeDescriptor;
        var props = propDescriptor.GetProperties()
            .OfType<PropertyDescriptor>()
            .Where(x => x.GetType().Name == "DbConnectionStringBuilderDescriptor")
            .ToList();

        Properties = props
            .ToDictionary(x => x.DisplayName.ToUpperInvariant(), x =>
            {
                var isSensitive = sensitivePropertiesHashSet.Contains(x.DisplayName);
                return new DbConnectionStringProperty(isSensitive, _connectionStringBuilder, x);
            });
    }

    public virtual string ToDisplayString()
    {
        var sensitiveProperties = Properties.Values.Where(x => x.IsSensitive);

        var removedProperties = new List<Tuple<string, object>>();
        foreach (var sensitiveProperty in sensitiveProperties)
        {
            var propertyName = sensitiveProperty.Name;
            if (!_connectionStringBuilder.ShouldSerialize(propertyName))
            {
                continue;
            }

            removedProperties.Add(new Tuple<string, object>(propertyName, _connectionStringBuilder[propertyName]));
            _connectionStringBuilder.Remove(propertyName);
        }

        var displayConnectionString = _connectionStringBuilder.ConnectionString;

        foreach (var prop in removedProperties)
        {
            _connectionStringBuilder[prop.Item1] = prop.Item2;
        }

        return displayConnectionString;
    }

    public override string ToString()
    {
        return _connectionStringBuilder.ConnectionString;
    }
}
