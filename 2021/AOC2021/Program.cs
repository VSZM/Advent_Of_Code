using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AOC2021
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var sw = new Stopwatch();
            sw.Start();
            var solver = new Day23(lines);
            sw.Stop();
            var precalc_time = sw.Elapsed;
            Console.WriteLine("Precalculations took: {0}", precalc_time);
            
            sw.Restart();
            var part1_solution = solver.SolvePart1();
            sw.Stop();
            var part1_time = sw.Elapsed;
            Console.WriteLine("{0} Part 1 Solution: < {1} > Took: {2}", solver.GetType().Name, part1_solution, part1_time);

            sw.Restart();
            var part2_solution = solver.SolvePart2();
            sw.Stop();
            var part2_time = sw.Elapsed;
            Console.WriteLine("{0} Part 2 Solution: < {1} > Took: {2}", solver.GetType().Name, part2_solution, part2_time);
            Console.WriteLine("Complete runtime: {0}", precalc_time + part1_time + part2_time);
        }
    }
}
