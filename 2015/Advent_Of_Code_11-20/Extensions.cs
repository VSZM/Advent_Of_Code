using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent_Of_Code_11_20
{
    static class Extensions
    {
        public static string ReplaceAt(this string str, string replacement, int position, int replace_length)
        {
            StringBuilder sb = new StringBuilder(str);
            sb.Remove(position, replace_length);
            sb.Insert(position, replacement);
            return sb.ToString();
        }

        // From: http://www.dotnetperls.com/levenshtein
        public static int Distance(this string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        // FROM: http://stackoverflow.com/a/1898744/1564252
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        {
            var enumerable = elements as T[] ?? elements.ToArray();
            return k == 0 ? new[] { new T[0] } :
              enumerable.SelectMany((e, i) =>
                enumerable.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }

        // FROM: http://stackoverflow.com/questions/3093622/generating-all-possible-combinations/3098381#3098381
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
            return sequences.Aggregate(
                emptyProduct,
                (accumulator, sequence) =>
                    from accseq in accumulator
                    from item in sequence
                    select accseq.Concat(new[] { item })
                );
        }

    }
}
