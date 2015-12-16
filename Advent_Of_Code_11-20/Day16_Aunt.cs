using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_Of_Code_11_20
{
    public class Day16Aunt: ISolvable
    {
        private static Aunt _referenceAunt = new Aunt("Sue 0: children: 3, cats: 7, samoyeds: 2, pomeranians: 3, akitas: 0, vizslas: 0, goldfish: 5, trees: 3, cars: 2, perfumes: 1");
        private static bool part2;

        private class Aunt : IComparable<Aunt>, IEquatable<Aunt>
        {
            private int _id;
            private readonly Dictionary<string, int> Properties = new Dictionary<string, int>();

            public Aunt(string line)
            {
                string[] splitted = line.Split();
                _id = int.Parse(splitted[1].Replace(':', ' '));
                for(int i= 2; i < splitted.Length;i+=2)
                    Properties.Add(splitted[i], int.Parse(splitted[i+1].Replace(',',' ')));
            }

            public int CompareTo(Aunt other)
            {
                throw new NotImplementedException();
            }

            public bool Equals(Aunt other)
            {
                return !Properties.Select(kv =>
                {
                    if (other.Properties.ContainsKey(kv.Key))
                    {
                        if (part2)
                        {
                            switch (kv.Key)
                            {
                                case "cats:":
                                    return kv.Value > other.Properties[kv.Key];
                                case "trees:":
                                    return kv.Value > other.Properties[kv.Key];
                                case "pomeranians:":
                                    return kv.Value < other.Properties[kv.Key];
                                case "goldfish:":
                                    return kv.Value < other.Properties[kv.Key];
                            }
                        }
                        return kv.Value == other.Properties[kv.Key];
                    }
                    return true;
                }).Contains(false);
            }

            public override string ToString()
            {
                return _id.ToString();
            }
        }

        public string Solve(string[] inputLines, bool isPart2)
        {
            part2 = isPart2;
            return inputLines.Select(t => new Aunt(t)).First(t => t.Equals(_referenceAunt)).ToString();
        }
    }
}