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
            BreedingLookup = new Dictionary<Tuple<int, int>, BigInteger>();
        }

        public IEnumerable<int> Fishes { get; private set; }
        public Dictionary<Tuple<int, int>, BigInteger> BreedingLookup { get; }

        private int Breed_Fish_For_X_Days_Simulated(int start_age, int x)
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

        private BigInteger Breed_Fish_For_X_Days_Recursive(int till_birth, int days)
        {
            var lookup_key = Tuple.Create(till_birth, days);
            if (BreedingLookup.ContainsKey(lookup_key))
                return BreedingLookup[lookup_key];
            BigInteger count = 1;
            while (days > 0)
            {
                till_birth--;
                if (till_birth == -1)
                {
                    till_birth = 6;
                    count += Breed_Fish_For_X_Days_Recursive(8, days-1);
                }
                days--;
            }
            BreedingLookup[lookup_key] = count;
            return count;
        }

        public object SolvePart1()
        {
            return Fishes.Select(till_birth => Breed_Fish_For_X_Days_Recursive(till_birth, 80))
                        .Aggregate(BigInteger.Add);
        }

        public object SolvePart2()
        {
            return Fishes.Select(till_birth => Breed_Fish_For_X_Days_Recursive(till_birth, 256))
                .Aggregate(BigInteger.Add);
        }
    }
}
