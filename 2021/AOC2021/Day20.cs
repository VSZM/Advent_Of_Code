using System;
using System.Numerics;
using System.Linq;
using System.Collections.Generic;

namespace AOC2021
{

    public class Day20 : ISolvable
    {

        public string Algo { get; }
        public Dictionary<(int X, int Y), bool> Grid { get; }


        public Day20(string[] lines)
        {
            Algo = lines[0];
            var length = lines[2].Length;
            Grid = new Dictionary<(int X, int Y), bool>();


            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Grid[(i, j)] = lines[i + 2][j] == '#';
                }
            }
        }

        private Dictionary<(int X, int Y), bool> Solve(int n)
        {
            var neighbours = new List<(int X, int Y)>() { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 0), (0, 1), (1, -1), (1, 0), (1, 1) };
            var grid = Grid;
            while (n > 0)
            {
                var minX = grid.Keys.Min(x => x.X);
                var minY = grid.Keys.Min(x => x.Y);
                var maxX = grid.Keys.Max(x => x.X);
                var maxY = grid.Keys.Max(x => x.Y);
                //Console.WriteLine(new string('-', 80));
                //Console.WriteLine("Initial State:");
                //PrintGrid(grid);
                var next_grid = new Dictionary<(int X, int Y), bool>();
                for (int i = minX - 1; i <= maxX + 1; i++)
                {
                    for (int j = minY - 1; j <= maxX + 1; j++)
                    {
                        var binary = new string(neighbours.Select(neigbour => Lookup(grid, i + neigbour.X, j + neigbour.Y, n) ? '1' : '0').ToArray());
                        next_grid[(i, j)] = Algo[Convert.ToInt32(binary, 2)] == '#';
                    }
                }
                grid = next_grid;

                //Console.WriteLine("After Algo:");
                //PrintGrid(grid);
                n--;
            }
            return grid;
        }

        private bool Lookup(Dictionary<(int X, int Y), bool> grid, int i, int j, int n)
        {
            if (grid.ContainsKey((i, j)))
                return grid[(i, j)];
            return n % 2 != 0 && Algo[0] == '#';
        }

        private void PrintGrid(Dictionary<(int X, int Y), bool> grid)
        {
            var minX = grid.Keys.Min(x => x.X);
            var minY = grid.Keys.Min(x => x.Y);
            var maxX = grid.Keys.Max(x => x.X);
            var maxY = grid.Keys.Max(x => x.Y);
            for (int i = minX; i <= maxX; i++)
            {
                for (int j = minY; j <= maxY; j++)
                {
                    Console.Write(grid[(i, j)] ? '#' : '.');
                }
                Console.WriteLine();
            }
        }

        public object SolvePart1()
        {
            var grid = Solve(2);
            return grid.Values.Count(x => x);
        }

        public object SolvePart2()
        {
            var grid = Solve(50);
            //PrintGrid(grid);
            return grid.Values.Count(x => x);
        }
    }
}

