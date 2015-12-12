using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Advent_Of_Code_11_20
{
    
    class Framework
    {
        [STAThread]
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();

            Day11Hacking solvable = new Day11Hacking();

            sw.Start();
            string part1_solution = Day11Hacking.Solve(new[] { "cqjxjnds" });
            sw.Stop();

            Console.WriteLine("Part 1 solution = '{0}'", part1_solution);
            Clipboard.SetText(part1_solution);
            Console.WriteLine("Part 1 solved in {0} ms. Clipboard contains the solution", sw.ElapsedMilliseconds);


            Console.WriteLine("Press any key to start part 2");
            Console.ReadKey();

            sw.Reset();
            Console.WriteLine("Starting Part 2 calculation");
            
            sw.Start();
            string part2_solution = Day11Hacking.Solve(new[] { Day11Hacking.Increment_String(part1_solution) });
            sw.Stop();

            Console.WriteLine("Part 2 solution = '{0}'", part2_solution);
            Clipboard.SetText(part2_solution);
            Console.WriteLine("Part 2 solved in {0} ms. Clipboard contains the solution", sw.ElapsedMilliseconds);
        }
    }
}
