namespace Orc.DataAccess.Registry;

using System;

internal sealed class RegistryKey : IRegistryKey
{
#pragma warning disable IDISP008
    private readonly Microsoft.Win32.RegistryKey _key;
#pragma warning restore IDISP008

    public RegistryKey(Microsoft.Win32.RegistryKey key)
    {
        ArgumentNullException.ThrowIfNull(key);

        _key = key;
    }

    public IRegistryKey? OpenSubKey(string name)
    {
        var winRegKey = _key.OpenSubKey(name, false);
        if (winRegKey is null)
        {
            return null;
        }

        return new RegistryKey(winRegKey);
    }

    public object? GetValue(string name)
    {
        return _key.GetValue(name);
    }

    public void Dispose()
    {
        _key.Dispose();
    }
}
