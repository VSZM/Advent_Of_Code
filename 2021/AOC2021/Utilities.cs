using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    using System;
    using System.Collections.Generic;

    public static class Utilities
    {
        public static IEnumerable<int> RangePython(int start, int stop, int step = 1)
        {
            if (step == 0)
                throw new ArgumentException("Parameter step cannot equal zero.");

            if (start < stop && step > 0)
            {
                for (var i = start; i < stop; i += step)
                {
                    yield return i;
                }
            }
            else if (start > stop && step < 0)
            {
                for (var i = start; i > stop; i += step)
                {
                    yield return i;
                }
            }
        }

        public static IEnumerable<int> RangePython(int stop)
        {
            return RangePython(0, stop);
        }

        public static string ToDebugString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return "{" + string.Join(",", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        }
    }
}
