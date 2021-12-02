using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_Of_Code_11_20
{
    class Day15Cooking : ISolvable
    {
        private static bool Next_Combination(List<int> combination)
        {
            int i;
            for (i = combination.Count - 1; i > 0 && combination[i] == 0; --i)
            { }// searching for the first non-zero element

            if (i == 0)// the input was the last one.
                return false;


            combination[i]--;
            combination[i - 1]++;
            for (int j = i; j < combination.Count - (combination.Count - i) / 2; ++j)
            {
                int tmp = combination[j];
                combination[j] = combination[combination.Count - 1 - (j - i)];
                combination[combination.Count - 1 - (j - i)] = tmp;
            }

            return true;
        }

        private class Ingredient
        {
            private readonly string _name;
            private readonly int _capacity;
            private readonly int _durability;
            private readonly int _flavor;
            private readonly int _texture;
            private readonly int _calories;

            public Ingredient(string name, int capacity, int durability, int flavor, int texture, int calories)
            {
                _name = name;
                _capacity = capacity;
                _flavor = flavor;
                _durability = durability;
                _calories = calories;
                _texture = texture;
            }

            public static int Combination_Score(IReadOnlyList<Ingredient> ingredients, IReadOnlyList<int> ratios)
            {
                int total_capacity = 0, total_durability = 0, total_flavor = 0, total_texture = 0;

                for (int i = 0; i < ingredients.Count; ++i)
                {
                    total_capacity += ingredients[i]._capacity * ratios[i];
                    total_durability += ingredients[i]._durability * ratios[i];
                    total_flavor += ingredients[i]._flavor * ratios[i];
                    total_texture += ingredients[i]._texture * ratios[i];
                }

                return Math.Max(0, total_capacity) * Math.Max(0, total_durability) * Math.Max(0, total_flavor) * Math.Max(0, total_texture);
            }

            public static int Calories(IReadOnlyList<Ingredient> ingredients, IReadOnlyList<int> ratios)
            {
                int total_calories = 0;

                for (int i = 0; i < ingredients.Count; ++i)
                {
                    total_calories += ingredients[i]._calories * ratios[i];
                }

                return total_calories;
            }
        }

        public string Solve(string[] inputLines, bool isPart2)
        {
            List<Ingredient> ingredients = inputLines.Select(t => t.Split()).Select(splittedLine =>
                                new Ingredient(
                                        splittedLine[0].Substring(0, splittedLine[0].Length - 1),
                                        int.Parse(splittedLine[2].Substring(0, splittedLine[2].Length - 1)),
                                        int.Parse(splittedLine[4].Substring(0, splittedLine[4].Length - 1)),
                                        int.Parse(splittedLine[6].Substring(0, splittedLine[6].Length - 1)),
                                        int.Parse(splittedLine[8].Substring(0, splittedLine[8].Length - 1)),
                                        int.Parse(splittedLine[10])
                                    )).ToList();

            List<int> combination = new List<int>(inputLines.Length);

            for (int i = 0; i < inputLines.Length - 1; i++)
            {
                combination.Add(0);
            }

            combination.Add(100);

            int max = 0;
            List<int> max_combination = null;

            do
            {
                int act_combination_value = Ingredient.Combination_Score(ingredients, combination);
                if (act_combination_value > max)
                {
                    if (isPart2)
                    {
                        if (Ingredient.Calories(ingredients, combination) == 500)
                        {
                            max = act_combination_value;
                            max_combination = new List<int>(combination);
                        }
                    }
                    else
                    {
                        max = act_combination_value;
                        max_combination = new List<int>(combination);
                    }
                }


                Console.WriteLine(string.Join(" ", combination));
            } while (Next_Combination(combination));

            Console.WriteLine("Best combination: " + string.Join(", ", max_combination));

            return max.ToString();
        }
    }
}
