namespace Orc.DataAccess;

using System;

public interface IRegistryKey : IDisposable
{
    IRegistryKey? OpenSubKey(string name);
    object? GetValue(string name);
}
