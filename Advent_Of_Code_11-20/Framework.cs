using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Advent_Of_Code_11_20
{
    
    class Framework
    {
        [STAThread]
        static void Main(string[] args)
        {
			string[] lines = File.ReadAllLines( "../../input.txt" );

            Stopwatch sw = new Stopwatch();

            ISolvable solvable = new Day16Aunt();

            sw.Start();
			string part1_solution = solvable.Solve( lines, false);
            sw.Stop();

            Console.WriteLine("Part 1 solution = '{0}'", part1_solution);
            Clipboard.SetText(part1_solution);
            Console.WriteLine("Part 1 solved in {0} ms. Clipboard contains the solution", sw.ElapsedMilliseconds);


            Console.WriteLine("Press any key to start part 2");
            Console.ReadKey();

            sw.Reset();
            Console.WriteLine("Starting Part 2 calculation");
            
            sw.Start();
			string part2_solution = solvable.Solve( lines, true );
            sw.Stop();

            Console.WriteLine("Part 2 solution = '{0}'", part2_solution);
            Clipboard.SetText(part2_solution);
            Console.WriteLine("Part 2 solved in {0} ms. Clipboard contains the solution", sw.ElapsedMilliseconds);
        }
    }
}
