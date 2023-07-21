namespace Orc.DataAccess.Tests;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;

public class TestDbConnectionStringBuilder : DbConnectionStringBuilder, ICustomTypeDescriptor
{
    private readonly List<PropertyDescriptor> _descriptors = new();

    public void AddPropertyDescription<T>(string name)
    {
        _descriptors.Add(new DbConnectionStringBuilderDescriptor(name, typeof(T), this));
    }

    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
    {
        return new PropertyDescriptorCollection(_descriptors.ToArray());
    }
}