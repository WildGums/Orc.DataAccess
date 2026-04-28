namespace Orc.DataAccess;

using Microsoft.Win32;

public interface IRegistryKeyService
{
    IRegistryKey OpenBaseKey(RegistryHive hKey, RegistryView view);
}
