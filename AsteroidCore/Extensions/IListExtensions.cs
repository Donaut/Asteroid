using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidCore.Extensions;

internal static class IListExtensions
{
    /// <summary>
    /// Te original <see cref="List{T}.Remove(T)"/> is O(N) but this one is O(1) at the cost of not preserving order.
    /// </summary>
    public static void RemoveBySwap<T>(this IList<T> list, int index)
    {
        list[index] = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
    }
}
