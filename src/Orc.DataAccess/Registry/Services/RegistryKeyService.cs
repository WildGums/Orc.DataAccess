namespace Orc.DataAccess.Registry;

using Microsoft.Win32;

internal class RegistryKeyService : IRegistryKeyService
{
    public IRegistryKey OpenBaseKey(RegistryHive hKey, RegistryView view)
    {
        return new RegistryKey(Microsoft.Win32.RegistryKey.OpenBaseKey(hKey, view));
    }
}
