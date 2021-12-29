using System;
using System.Numerics;
using System.Linq;
using System.Collections.Generic;

namespace AOC2021
{
    public class DayTwenty
    {
        private readonly string _algorithm;
        private Dictionary<(int X, int Y), bool> _grid;
        private Dictionary<(int X, int Y), bool> _original;
        private List<(int X, int Y)> _neighbourOffsets;

        public DayTwenty(string[] input)
        {
            _algorithm = input[0];
            _grid = input[2..].SelectMany((x, i) => x.Select((y, j) => (j, i, y))).ToDictionary(x => (x.j, x.i), x => x.y == '#');
            _original = _grid;
            _neighbourOffsets = new List<(int X, int Y)> { (-1, -1), (0, -1), (1, -1), (-1, 0), (0, 0), (1, 0), (-1, 1), (0, 1), (1, 1), };
        }

        public void Process()
        {
            Console.WriteLine($"Part 1: {IterateTwice()}");
            Console.WriteLine($"Part 2: {IterateFifty()}");
        }

        public int IterateTwice()
        {
            Enumerable.Range(0, 2).ToList().ForEach(x => Expand());
            return _grid.Count(x => x.Value);
        }

        public int IterateFifty()
        {
            _grid = _original;
            Enumerable.Range(0, 50).ToList().ForEach(x => Expand());
            return _grid.Count(x => x.Value);

        }

        private void Expand()
        {
            var minX = _grid.Keys.Min(x => x.X);
            var minY = _grid.Keys.Min(x => x.Y);
            var maxX = _grid.Keys.Max(x => x.X);
            var maxY = _grid.Keys.Max(x => x.Y);

            Dictionary<(int X, int Y), bool> newGrid = new();

            for (int y = minY - 1; y <= maxY + 1; y++)
            {
                for (int x = minX - 1; x <= maxX + 1; x++)
                {
                    var miniGrid = _neighbourOffsets.Select(z => (z.X + x, z.Y + y)).ToList();
                    var binary = new string(miniGrid.Select(z => IsLit(z, minX + 1) ? '1' : '0').ToArray());
                    newGrid[(x, y)] = _algorithm[Convert.ToInt32(binary, 2)] == '#';
                }
            }

            _grid = newGrid;
        }

        private bool IsLit((int X, int Y) coord, int minX)
        {
            if (!_grid.ContainsKey(coord) && minX % 2 == 0 && _algorithm[0] == '#')
            {
                return true;
            }
            if (!_grid.ContainsKey(coord))
            {
                return false;
            }
            return _grid[coord];
        }
    }

    public class Day20 : ISolvable
    {

        public Day20(string[] lines)
        {
            this.Helper = new DayTwenty(lines);
            Algo = lines[0];
            var length = lines[2].Length;
            Grid = new int[length, length];


            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    Grid[i, j] = lines[i + 2][j] == '#' ? 1 : 0;
                }
            }
        }

        private int[,] Solve(int n)
        {
            var grid = Grid;
            while (n > 0)
            {
                Console.WriteLine(new string('-', 80));
                Console.WriteLine("Initial State:");
                Utilities.PrintGridNumbers(grid);
                grid = Add_Border(grid, 2, n % 2 == 0);
                Console.WriteLine("After padding:");
                Utilities.PrintGridNumbers(grid);
                var next_grid = (int[,])grid.Clone();
                for (int i = 1; i < grid.GetLength(0) - 1; i++)
                {
                    for (int j = 1; j < grid.GetLength(1) - 1; j++)
                    {
                        var binary = grid[i - 1, j - 1].ToString() + grid[i - 1, j].ToString() + grid[i - 1, j + 1].ToString() +
                                     grid[i, j - 1].ToString() + grid[i, j].ToString() + grid[i, j + 1].ToString() +
                                     grid[i + 1, j - 1].ToString() + grid[i + 1, j].ToString() + grid[i + 1, j + 1].ToString();
                        next_grid[i, j] = Algo[Convert.ToInt32(binary, 2)] == '#' ? 1 : 0;
                    }
                }
                grid = next_grid;
                Console.WriteLine("After Algo:");
                Utilities.PrintGridNumbers(grid);
                n--;
            }
            return grid;
        }

        /**
         * 
         * 12
         * 34
         * 
         * -> 1
         * 
         * 0000
         * 0120
         * 0340
         * 0000
         * */

        private int[,] Add_Border(int[,] grid, int border_size, bool is_odd)
        {
            var length = grid.GetLength(0);
            var new_grid = new int[length + 2 * border_size, length + 2 * border_size];

            for (int i = 0; i < length + 2 * border_size; i++)
            {
                for (int j = 0; j < length + 2 * border_size; j++)
                {
                    if (i < border_size || j < border_size || i >= border_size + length || j >= border_size + length)
                        new_grid[i, j] = is_odd && Algo[0] == '#' ? 1 : 0;
                    else
                        new_grid[i, j] = grid[i - border_size, j - border_size];
                }
            }

            length += border_size * 2;

            return new_grid;
        }

        public DayTwenty Helper { get; }
        public string Algo { get; }
        public int[,] Grid { get; }

        public object SolvePart1()
        {
            var grid = Solve(2);
            var  mine = Enumerable.Range(0, grid.GetLength(0)).Select(i => Enumerable.Range(0, grid.GetLength(1)).Select(j => grid[i, j]).Sum()).Sum();
            return string.Format("Helper: {0}, Mine: {1}", Helper.IterateTwice(), mine);
        }

        public object SolvePart2()
        {
            throw new NotImplementedException();
        }
    }
}

