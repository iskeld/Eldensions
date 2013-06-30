using System.Collections.Generic;
using System.Linq;

namespace EldSharp.Eldensions.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool HasDuplicates<TItem>(this IEnumerable<TItem> list)
        {
            return list.ToList().HasDuplicates();
        }

        public static IEnumerable<TItem> GetDuplicatedItems<TItem>(this IEnumerable<TItem> list)
        {
            HashSet<TItem> set = new HashSet<TItem>();
            return list.Where(item => !set.Add(item));
        }
    }
}
