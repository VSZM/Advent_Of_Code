using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AOC2021
{
    public class Day14 : ISolvable
    {
        public Day14(string[] lines)
        {
            Template = lines[0];
            Rules = lines.Skip(2).Select(line => line.Split(" -> "))
                .ToDictionary(pair => Tuple.Create(pair[0][0], pair[0][1]), pair => pair[1][0]);
            Cache = new Dictionary<Tuple<string, BigInteger>, Dictionary<char, BigInteger>>();
        }

        public string Template { get; }
        public Dictionary<Tuple<char, char>, char> Rules { get; }
        public Dictionary<Tuple<string, BigInteger>, Dictionary<char, BigInteger>> Cache { get; }

        private Dictionary<char, BigInteger> GetElementDict()
        {
            var ret = new Dictionary<char, BigInteger>();
            foreach (var c in Rules.Values)
            {
                if (!ret.ContainsKey(c))
                {
                    ret[c] = 0;
                }
            }
            return ret;
        }

        private Dictionary<char, BigInteger> GetElementCount(string template, BigInteger steps)
        {
            var counts = GetElementDict();
            if (steps == 0)
            {
                foreach (var c in template)
                {
                    counts[c]++;
                }
            }
            else
            {
                for (int i = 0; i < template.Length - 1; i++) // How to rewrite this loop to be more simple?
                {
                    Dictionary<char, BigInteger> count = null;
                    var subtemplate = new string(new char[] { template[i], Rules[Tuple.Create(template[i], template[i + 1])], template[i + 1] });
                    if (i > 0)
                    {
                        var cache_key = Tuple.Create(subtemplate, steps - 1);
                        if (Cache.ContainsKey(cache_key))
                            count = Cache[cache_key];
                        else
                        {
                            count = GetElementCount(subtemplate, steps - 1);
                            // This one line is frustrating and I think this is what overcomplicates this entire funciton
                            // The internal pairs have duplicates in them the way I generate them. For example NNCB becomes NCN, NBC, CHB thus I have to remove the duplicates..
                            count[template[i]] -= 1; 
                            Cache[cache_key] = count;
                        }
                    }
                    else
                    {
                        count = GetElementCount(subtemplate, steps - 1);
                    }
                    foreach (var key in count.Keys)
                    {
                        counts[key] += count[key];
                    };
                }
            }
            return counts;
        }

        private object Solve(BigInteger steps)
        {
            var counts = GetElementCount(Template, steps);
            Console.WriteLine(counts.ToDebugString());
            return counts.Values.Max() - counts.Values.Min();
        }

        public object SolvePart1()
        {
            return Solve(10);
        }

        public object SolvePart2()
        {
            Cache.Clear();
            return Solve(40);
        }
    }
}
