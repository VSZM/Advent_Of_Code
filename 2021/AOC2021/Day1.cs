using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    class Day1 : ISolvable
    {
        public Day1(string[] lines)
        {
            Lines = lines;
            Numbers = Lines.Select(item => int.Parse(item)).ToList();
        }

        public string[] Lines { get; }
        public List<int> Numbers { get; }

        public object solve_part_1()
        {
            var previous = Numbers[0];
            var increment_counter = 0;
            Numbers.Skip(1).ToList().ForEach(number =>
            {
                if (previous < number)
                {
                    increment_counter++;
                }
                previous = number;
            });
            return increment_counter;
        }

        public object solve_part_2()
        {
            var increment_counter = 0;
            for (int i = 3; i < Numbers.Count; i++)
            {
                var previous = Numbers[i - 3] + Numbers[i - 2] + Numbers[i - 1];
                var current = Numbers[i - 2] + Numbers[i - 1] + Numbers[i];
                if (previous < current)
                {
                    increment_counter++;
                }
            }
            return increment_counter;
        }
    }
}
