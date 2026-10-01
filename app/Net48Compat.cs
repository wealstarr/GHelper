using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GHelper;

internal static class Net48Compat
{
    public static long TickCount64 => unchecked((uint)Environment.TickCount);

    public static int ProcessId => System.Diagnostics.Process.GetCurrentProcess().Id;
}

namespace System.Collections.Generic
{
    internal static class DictionaryCompatExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.TryGetValue(key, out var value) ? value : default(TValue);
        }
    }
}

namespace System.Linq
{
    internal static class LinqCompatExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seen = new HashSet<TKey>();
            foreach (var item in source)
                if (seen.Add(keySelector(item))) yield return item;
        }
    }
}

namespace GHelper
{
    internal static class TaskCompatExtensions
    {
        public static async Task WaitAsync(this Task task, CancellationToken cancellationToken)
        {
            if (!cancellationToken.CanBeCanceled) { await task.ConfigureAwait(false); return; }
            var completed = await Task.WhenAny(task, Task.Delay(Timeout.Infinite, cancellationToken)).ConfigureAwait(false);
            await completed.ConfigureAwait(false);
        }
    }
}
