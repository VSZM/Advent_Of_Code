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
            Cache = new Dictionary<Tuple<char, char, int>, Dictionary<char, BigInteger>>();
        }

        public string Template { get; }
        public Dictionary<Tuple<char, char>, char> Rules { get; }
        public Dictionary<Tuple<char, char, int>, Dictionary<char, BigInteger>> Cache { get; }

        private Dictionary<char, BigInteger> GetElementDict(params char[] args)
        {
            var ret = new Dictionary<char, BigInteger>();
            foreach (var c in Rules.Values)
            {
                if (!ret.ContainsKey(c))
                {
                    ret[c] = 0;
                }
            }
            foreach (var c in args)
            {
                ret[c]++;
            }

            return ret;
        }

        private Dictionary<char, BigInteger> GetElementCount(char left, char right, int steps)
        {
            var key = Tuple.Create(left, right, steps);
            if (Cache.ContainsKey(key))
            {
                return Cache[key];
            }
            var counts = GetElementDict();
            if (steps == 0)
            {
                return counts;
            }
            var middle = Rules[Tuple.Create(left, right)];
            counts[middle]++;
            var counter_left = GetElementCount(left, middle, steps - 1);
            var counter_right = GetElementCount(middle, right, steps - 1);
            var result = counts.Combine(counter_left).Combine(counter_right);
            Cache[key] = result;
            return result;
        }

        private object Solve(int steps)
        {
            var counts = GetElementDict(Template.ToCharArray());
            for (int i = 0; i < Template.Length-1; i++)
            {
                Console.WriteLine("{0}{1}",Template[i], Template[i+1]);
                var count = GetElementCount(Template[i], Template[i+1], steps);
                counts = counts.Combine(count);
            }
            Console.WriteLine(counts.ToDebugString());
            return counts.Values.Max() - counts.Values.Min();
        }

        public object SolvePart1()
        {
            return Solve(10);
        }

        public object SolvePart2()
        {
            return Solve(40);
        }
    }
}
