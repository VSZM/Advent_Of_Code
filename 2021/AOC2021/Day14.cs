using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC2021
{
    public class Day14 : ISolvable
    {
        public Day14(string[] lines)
        {
            Template = lines[0];
            Rules = lines.Skip(2).Select(line => line.Split(" -> "))
                .ToDictionary(pair => Tuple.Create(pair[0][0], pair[0][1]), pair => pair[1]);
        }

        public string Template { get; }
        public Dictionary<Tuple<char, char>, string> Rules { get; }


        private object Solve(int steps)
        {
            var chain = Template;
            for (int n = 0; n < steps; n++)
            {
                var sb = new StringBuilder();
                sb.Append(chain[0]);
                for (int i = 1; i < chain.Length; i++)
                {
                    sb.Append(Rules[Tuple.Create(chain[i - 1], chain[i])]);
                    sb.Append(chain[i]);
                }
                var count = chain.GroupBy(c => c).Select(group => new
                {
                    C = group.Key,
                    Count = group.Count()
                });
                Console.WriteLine(string.Format("B: {0}, H: {1}, B-H: {2}",
                                                count.Where(row => row.C == 'B').Select(row => row.Count).FirstOrDefault(),
                                                count.Where(row => row.C == 'H').Select(row => row.Count).FirstOrDefault(),
                                                count.Where(row => row.C == 'B').Select(row => row.Count).FirstOrDefault() -
                                                count.Where(row => row.C == 'H').Select(row => row.Count).FirstOrDefault()));
                chain = sb.ToString();
            }


            var counts = chain.GroupBy(c => c).Select(group => new
            {
                C = group.Key,
                Count = group.Count()
            });
            return counts.Select(row => row.Count).Max() - counts.Select(row => row.Count).Min();
        }

        public object SolvePart1()
        {
            return Solve(40);
        }

        public object SolvePart2()
        {
            return "asd";
            //return Solve(40);
        }
    }
}
