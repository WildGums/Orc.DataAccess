namespace Orc.DataAccess;

using Microsoft.Win32;

internal interface IRegistryKeyService
{
    IRegistryKey OpenBaseKey(RegistryHive hKey, RegistryView view);
}
