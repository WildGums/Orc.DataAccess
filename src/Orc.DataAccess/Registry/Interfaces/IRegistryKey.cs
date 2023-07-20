namespace Orc.DataAccess.Registry;

using System;

internal interface IRegistryKey : IDisposable
{
    IRegistryKey? OpenSubKey(string name);
    object? GetValue(string name);
}
