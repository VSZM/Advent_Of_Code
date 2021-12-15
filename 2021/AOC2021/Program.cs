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
            var solver = new Day14(lines);
            Console.WriteLine(string.Format("{0} Part 1 Solution: < {1} >", solver.GetType().Name, solver.SolvePart1()));
            Console.WriteLine(string.Format("{0} Part 2 Solution: < {1} >", solver.GetType().Name, solver.SolvePart2()));
        }
    }
}
