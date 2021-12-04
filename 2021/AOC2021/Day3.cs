using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    class Day3 : ISolvable
    {
        public Day3(string[] lines)
        {
            Lines = lines;
        }

        public string[] Lines { get; }

        public object solve_part_1()
        {
            string gamma = "";
            string epsilon = "";

            for (int i = 0; i < Lines[0].Length; ++i)
            {
                gamma += Lines.Select(line => line[i])
                            .GroupBy(c => c)
                            .OrderByDescending(group => group.Count())
                            .Take(1)
                            .Select(group => group.Key).First();
                epsilon += Lines.Select(line => line[i])
                            .GroupBy(c => c)
                            .OrderBy(group => group.Count())
                            .Take(1)
                            .Select(group => group.Key).First();
            }
            return Convert.ToInt32(gamma, 2) * Convert.ToInt32(epsilon, 2);
        }

        public object solve_part_2()
        {
            var oxygen_candidates = new List<string>(Lines);
            int pos = 0;
            while (oxygen_candidates.Count > 1)
            {
                var count1 = oxygen_candidates.Count(str => str[pos] == '1');
                var count0 = oxygen_candidates.Count - count1;
                if (count1 >= count0)
                {
                    oxygen_candidates = oxygen_candidates.Where(str => str[pos] == '1').ToList();
                }
                else
                {
                    oxygen_candidates = oxygen_candidates.Where(str => str[pos] == '0').ToList();
                }
                pos++;
            }
            var co2_candidates = new List<string>(Lines);
            pos = 0;
            while (co2_candidates.Count > 1)
            {
                var count1 = co2_candidates.Count(str => str[pos] == '1');
                var count0 = co2_candidates.Count - count1;
                if (count1 < count0)
                {
                    co2_candidates = co2_candidates.Where(str => str[pos] == '1').ToList();
                }
                else
                {
                    co2_candidates = co2_candidates.Where(str => str[pos] == '0').ToList();
                }
                pos++;
            }

            return Convert.ToInt32(oxygen_candidates[0], 2) * Convert.ToInt32(co2_candidates[0], 2);
        }
    }
}
