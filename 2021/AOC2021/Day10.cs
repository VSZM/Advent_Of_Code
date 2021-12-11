using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AOC2021
{
    public class Day10 : ISolvable
    {

        private static readonly Dictionary<char, char> StartToEnd = new Dictionary<char, char>()
        {
            ['<'] = '>',
            ['('] = ')',
            ['{'] = '}',
            ['['] = ']'
        };
        private static readonly Dictionary<char, char> EndToStart = StartToEnd.ToDictionary(item => item.Value, item => item.Key);
        private static readonly Dictionary<char, int> PointLookup = new Dictionary<char, int>()
        {
            [')'] = 3,
            [']'] = 57,
            ['}'] = 1197,
            ['>'] = 25137
        };
        private static readonly Dictionary<char, int> PointLookup2 = new Dictionary<char, int>()
        {
            [')'] = 1,
            [']'] = 2,
            ['}'] = 3,
            ['>'] = 4
        };

        public Day10(string[] lines)
        {
            Lines = lines;
        }

        public string[] Lines { get; }

        public object SolvePart1()
        {
            int sum = 0;
            foreach (var line in Lines)
            {
                Stack<char> brackets = new Stack<char>();
                brackets.Push(line[0]);
                foreach (var ch in line.Skip(1))
                {

                    if (StartToEnd.ContainsKey(ch))
                    {
                        brackets.Push(ch);
                    }
                    else
                    {
                        if (ch == StartToEnd[brackets.Peek()])
                        {
                            brackets.Pop();
                        }
                        else
                        {
                            sum += PointLookup[ch];
                            break;
                        }
                    }
                }
            }
            return sum;
        }

        public object SolvePart2()
        {
            var scores = new List<BigInteger>();
            foreach (var line in Lines)
            {
                Stack<char> brackets = new Stack<char>();
                brackets.Push(line[0]);
                foreach (var ch in line.Skip(1))
                {

                    if (StartToEnd.ContainsKey(ch))
                    {
                        brackets.Push(ch);
                    }
                    else
                    {
                        if (ch == StartToEnd[brackets.Peek()])
                        {
                            brackets.Pop();
                        }
                        else
                        {
                            brackets.Clear();
                            break;
                        }
                    }
                }

                BigInteger linesum = 0;
                while (brackets.Count > 0)
                {
                    var bracket = brackets.Pop();
                    linesum *= 5;
                    linesum += PointLookup2[StartToEnd[bracket]];
                }

                if (linesum > 0)
                    scores.Add(linesum);
            }


            return scores.OrderBy(score => score).Skip(scores.Count / 2).First();
        }
    }
}
