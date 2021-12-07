using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AOC2021
{
    public class Day6 : ISolvable
    {

        public Day6(string[] lines)
        {
            Fishes = lines[0].Split(',').Select(str => int.Parse(str));
            for(int i = 0; i < 100; ++i)
            {
                Console.WriteLine(string.Format("Fishies Count after {0} Days: {1}", i, Breed_Fish_For_X_Days(0, i)));
            }
        }

        public IEnumerable<int> Fishes { get; private set; }

        private int Breed_Fish_For_X_Days(int start_age, int x)
        {
            List<int> fishies = new List<int> { start_age };
            for (int day = 0; day < x; day++)
            {
                var current_gen_count = fishies.Count;
                for (int i = 0; i < current_gen_count; i++)
                {
                    fishies[i]--;
                    if (fishies[i] < 0)
                    {
                        fishies[i] = 6;
                        fishies.Add(8);
                    }
                }
            }
            return fishies.Count();
        }

        public object solve_part_1()
        {
            var breeding_lookup = Enumerable.Range(0, 7).ToDictionary(start => start, start => Breed_Fish_For_X_Days(start, 80));
            return Fishes.Select(start => breeding_lookup[start]).Select(exp => new BigInteger(exp))
                        .Aggregate(BigInteger.Add);
        }

        public object solve_part_2()
        {
            return 0;
            var breeding_lookup = Enumerable.Range(0, 7).ToDictionary(start => start, start => Breed_Fish_For_X_Days(start, 256));
            return Fishes.Select(start => breeding_lookup[start]).Select(exp => new BigInteger(exp))
                        .Aggregate(BigInteger.Add);
        }
    }
}
