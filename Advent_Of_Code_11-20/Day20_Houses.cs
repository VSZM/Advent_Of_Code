using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_11_20
{
    class Day20Houses : ISolvable
    {
        public string Solve(string[] inputLines, bool isPart2)
        {
            int N = int.Parse(inputLines[0]) / (isPart2 ? 11 : 10);
            int[] houses = new int[N];

            for (int i = 1; i < houses.Length; i++)
            {
                for (int j = i; j < houses.Length && (!isPart2 || j < 51 * i); j += i)
                {
                    houses[j] += i;
                }
            }

            for (int i = 0; i < N; ++i)
                if (houses[i] >= N)
                    return i.ToString();

            return "Should work doe..";
        }
    }
}
