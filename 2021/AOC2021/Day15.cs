using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Drawing;

namespace AOC2021
{
    class Path
    {
        public Path(List<Point> points, int cost, double h)
        {
            PathSoFar = points;
            CostSoFar = cost;
            H = h;
        }

        public List<Point> PathSoFar { get; set; }
        public int CostSoFar { get; set; }
        public double H { get; set; }

        public override string ToString()
        {
            return string.Format("Cost: {0}, H: {1}, Path: {2}", CostSoFar, H, PathSoFar);
        }
    }

    class Day15 : ISolvable
    {
        private List<Point> DIRECTIONS = new List<Point>() { new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1) };

        public Day15(string[] lines)
        {
            Rows = lines.Length;
            Cols = lines[0].Length;
            Grid = new int[Rows, Cols];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    Grid[i, j] = int.Parse(lines[i][j].ToString());
                }
            }
        }

        public int Rows { get; set; }
        public int Cols { get; set; }
        public int[,] Grid { get; set; }

        private double h1(Point p)
        {
            return (Rows - p.X - 1) + (Cols - p.Y - 1) * 2.5;
        }

        private bool IsEnd(Point p)
        {
            return p.X == Rows - 1 && p.Y == Cols - 1;
        }

        private List<Point> GetUnvisitedNeigbours(Point p, List<Point> path)
        {
            var ret = new List<Point>();

            foreach (var dir in DIRECTIONS)
            {
                var new_p = new Point(p.X + dir.X, p.Y + dir.Y);
                if (new_p.X >= 0 && new_p.X < Rows && new_p.Y >= 0 && new_p.Y < Cols && !path.Contains(new_p))
                {
                    ret.Add(new_p);
                }
            }


            return ret;
        }

        // A*
        private int Solve()
        {
            var queue = new PriorityQueue<Path, double>();
            queue.Enqueue(new Path(new List<Point> { new Point(0, 0) }, 0, h1(new Point(0, 0))), h1(new Point(0, 0)));
            var optimal_costs = new Dictionary<Point, int>();
            optimal_costs.Add(new Point(0, 0), 0);
            int nodes_visited = 0;


            while (queue.Count > 0)
            {
                nodes_visited++;
                var path = queue.Dequeue();
                var tip = path.PathSoFar.Last();
                if (IsEnd(tip))
                {
                    Console.WriteLine("Search Cost: {0}", nodes_visited);
                    return path.CostSoFar;
                }

                GetUnvisitedNeigbours(tip, path.PathSoFar).ForEach(p =>
                {
                    int current_best = int.MaxValue;
                    if (optimal_costs.ContainsKey(p))
                        current_best = optimal_costs[p];

                    var new_cost = path.CostSoFar + Grid[p.X, p.Y];
                    if (new_cost < current_best)
                    {
                        optimal_costs[p] = new_cost;
                        var new_path = new List<Point>(path.PathSoFar);
                        new_path.Add(p);
                        var new_path_obj = new Path(new_path, new_cost, h1(p));
                        queue.Enqueue(new_path_obj, new_path_obj.CostSoFar + new_path_obj.H);
                    }
                });
            }

            return -1;
        }

        public object SolvePart1()
        {
            return Solve();
        }

        public object SolvePart2()
        {
            var new_grid = new int[Rows * 5, Cols * 5];
            for (int i = 0; i < Rows; i++) // Copying into first section
            {
                for (int j = 0; j < Cols; j++)
                {
                    new_grid[i, j] = Grid[i, j];
                }
            }

            for (int col = 1; col < 5; col++) // Expanding first row
            {
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Cols; j++)
                    {
                        new_grid[i, j + col * Cols] = new_grid[i, j + (col - 1) * Cols] + 1;
                        if (new_grid[i, j + col * Cols] == 10)
                        {
                            new_grid[i, j + col * Cols] = 1;
                        }
                    }
                }
            }


            for (int row = 1; row < 5; row++)// Expanding rows
            {
                for (int col = 0; col < 5; col++)
                {
                    for (int i = 0; i < Rows; i++)
                    {
                        for (int j = 0; j < Cols; j++)
                        {
                            new_grid[i + row * Rows, j + col * Cols] = new_grid[i + (row - 1) * Rows, j + col * Cols] + 1;
                            if (new_grid[i + row * Rows, j + col * Cols] == 10)
                            {
                                new_grid[i + row * Rows, j + col * Cols] = 1;
                            }
                        }
                    }
                }
            }

            //Utilities.PrintGridNumbers(new_grid);
            Grid = new_grid;
            Rows = Rows * 5;
            Cols = Cols * 5;
            return Solve();
        }
    }
}
