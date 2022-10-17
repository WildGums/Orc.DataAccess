namespace Orc.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catel.Reflection;

    public static class TypeExtensions
    {
        public static IList<Type> GetAllAssignableFrom(this Type type)
        {
            var descendantTypes = new List<Type>();
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var loadedAssembly in loadedAssemblies)
            {
                try
                {
                    var descendantTypesFromCurrentAssembly = loadedAssembly.GetTypesEx()
                        .Where(x => type.IsAssignableFrom(x) && x.IsClass && !x.IsAbstract && !x.IsGenericType)
                        .ToList();

                    descendantTypes.AddRange(descendantTypesFromCurrentAssembly);
                }
                catch
                {
                    //do nothing
                }
            }

            return descendantTypes;
        }
    }
}
