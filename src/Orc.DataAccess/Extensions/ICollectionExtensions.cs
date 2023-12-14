namespace Orc.DataAccess;

using System;
using System.Collections.Generic;
using System.Linq;

public static class ICollectionExtensions
{
    public static TTarget FindTypeOrCreateNew<T, TTarget>(this ICollection<T> collection, Func<TTarget> func)
        where TTarget : T
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(func);

        var result = collection.OfType<TTarget>().FirstOrDefault();
        if (result is not null)
        {
            return result;
        }

        result = func();
        collection.Add(result);
        return result;
    }
}