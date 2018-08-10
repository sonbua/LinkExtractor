using System;
using System.Collections.Generic;
using EnsureThat;

namespace Brick.IO
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            EnsureArg.IsNotNull(source, nameof(source));
            EnsureArg.IsNotNull(action, nameof(action));

            foreach (var item in source)
            {
                action(item);
            }
        }
    }
}