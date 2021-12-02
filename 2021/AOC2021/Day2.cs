using System;
namespace AOC2021
{
    public class Day2 : ISolvable
    {
        public Day2(string[] lines)
        {
            this.Instructions = lines;
        }

        public string[] Instructions { get; }

        public string solve_part_1()
        {
            int depth = 0;
            int h = 0;
            foreach(var instruction in Instructions)
            {
                int num = int.Parse(instruction.Split()[1]);
                if(instruction.StartsWith("forward"))
                {
                    h += num;
                } else if (instruction.StartsWith("down"))
                {
                    depth += num;
                } else
                {
                    depth -= num;
                }
            }

            return (h * depth).ToString();
        }

        public string solve_part_2()
        {
            int depth = 0;
            int h = 0;
            int aim = 0;
            foreach (var instruction in Instructions)
            {
                int num = int.Parse(instruction.Split()[1]);
                if (instruction.StartsWith("forward"))
                {
                    h += num;
                    depth += aim * num;
                }
                else if (instruction.StartsWith("down"))
                {
                    aim += num;
                }
                else
                {
                    aim -= num;
                }
            }

            return (h * depth).ToString();
        }
    }
}
