using System;
using System.IO;
using System.Linq;

namespace AOC2021
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var solver = new Day1(lines);
            Console.WriteLine(string.Format("{0} Part 1 Solution: < {1} >", solver.GetType().Name, solver.solve_part_1()));
            Console.WriteLine(string.Format("{0} Part 2 Solution: < {1} >", solver.GetType().Name, solver.solve_part_2()));
        }
    }
}
