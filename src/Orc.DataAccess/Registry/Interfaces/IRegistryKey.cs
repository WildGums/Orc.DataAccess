namespace Orc.DataAccess;

using System;

internal interface IRegistryKey : IDisposable
{
    IRegistryKey? OpenSubKey(string name);
    object? GetValue(string name);
}
