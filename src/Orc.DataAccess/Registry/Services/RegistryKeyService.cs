namespace Orc.DataAccess;

using Microsoft.Win32;

internal class RegistryKeyService : IRegistryKeyService
{
    public IRegistryKey OpenBaseKey(RegistryHive hKey, RegistryView view)
    {
        return new DataAccess.RegistryKey(Microsoft.Win32.RegistryKey.OpenBaseKey(hKey, view));
    }
}
