using System.Collections.Generic;
using System.Linq;

namespace Test_Solution1.Common.Extension
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmptyCheck<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
    }
}