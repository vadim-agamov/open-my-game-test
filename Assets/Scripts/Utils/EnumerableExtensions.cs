using System;
using System.Collections.Generic;

namespace Modules.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var item in self)
            {
                action(item);
            }
        }
    }
}