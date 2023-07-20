namespace Orc.DataAccess.Tests;

using System;
using System.ComponentModel;

public class DbConnectionStringBuilderDescriptor : PropertyDescriptor
{
    private readonly string _name;
    private readonly TestDbConnectionStringBuilder _builder;

    private object? _value = string.Empty;

    public DbConnectionStringBuilderDescriptor(string name, Type type, TestDbConnectionStringBuilder builder)
        : base(name, null)
    {
        _name = name;
        _builder = builder;

        PropertyType = type;
    }

    public override Type ComponentType { get; }
    public override bool IsReadOnly => false;
    public override Type PropertyType { get; }

    public override bool CanResetValue(object component) => true;

    public override object? GetValue(object? component) => _value;

    public override void ResetValue(object component)
    {
        //Do nothing
    }

    public override void SetValue(object? component, object? value)
    {
        _value = value;
        _builder[_name] = value;
    }

    public override bool ShouldSerializeValue(object component) => false;
}