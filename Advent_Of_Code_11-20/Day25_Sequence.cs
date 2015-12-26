using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_11_20
{
    class Day25Sequence : ISolvable
    {
        private static long Get_Code(int row, int column)
        {
            int N = column * (column + 1) / 2;
            for (int i = 1; i < row; i++)
            {
                N += column++;
            }

            long ret = 20151125;

            for (int i = 1; i < N; i++)
                checked
                {
                    ret = (ret * 252533) % 33554393;
                }

            return ret;
        }

        public string Solve(string[] inputLines, bool isPart2)
        {
            var splitted_line = inputLines[0].Split();
            int column = int.Parse(splitted_line.Last().TrimEnd(new[] { '.' }));
            int row = int.Parse(splitted_line[splitted_line.Length - 3].TrimEnd(new[] { ',' }));

            //for (int r = 1; r <= 6; r++)
            //{
            //    for (int c = 1; c <= 6; c++)
            //    {
            //        Console.Write(Get_Code(r, c) + "\t");
            //    }
            //    Console.WriteLine();
            //}

            return Get_Code(row, column).ToString();
        }
    }
}
