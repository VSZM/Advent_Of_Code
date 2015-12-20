using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_11_20
{
    class Day17StorageCombinations : ISolvable
    {
        public List<List<T>> ProduceWithoutRecursion<T>(List<T> allValues)
        {
            var collection = new List<List<T>>();
            for (int counter = 0; counter < (1 << allValues.Count); ++counter)
            {
                List<T> combination = new List<T>();
                for (int i = 0; i < allValues.Count; ++i)
                {
                    if ((counter & (1 << i)) == 0)
                        combination.Add(allValues[i]);
                }

                collection.Add(combination);
            }
            return collection;
        }


        public string Solve(string[] inputLines, bool isPart2)
        {
            List<int> containers = inputLines.Select(int.Parse).ToList();

            if (!isPart2)
                return ProduceWithoutRecursion(containers).Count(combination => combination.Sum() == 150).ToString();

            var combinations = ProduceWithoutRecursion(containers);

            int min_count =
                combinations.Where(combination => combination.Sum() == 150).Min(combination => combination.Count);

            return ProduceWithoutRecursion(containers).Count(combination => combination.Sum() == 150 && combination.Count == min_count).ToString();
        }
    }
}
