namespace Orc.DataAccess.Database
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ConnectToProviderAttribute : Attribute
    {
        public ConnectToProviderAttribute(string providerInvariantName)
        {
            ProviderInvariantName = providerInvariantName;
        }

        public string ProviderInvariantName { get; }
    }
}
