using System.Collections.Generic;
using System.Linq;

namespace EldSharp.Eldensions.Extensions
{
    public static class CollectionExtensions
    {
        public static bool HasDuplicates<TItem>(this ICollection<TItem> list)
        {
            return list.Count != list.Distinct().Count();
        }
    }
}
