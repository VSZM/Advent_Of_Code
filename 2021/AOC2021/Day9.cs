using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AOC2021
{
    public class Day9 : ISolvable
    {

        public Day9(string[] lines)
        {
            RowNum = lines.Length;
            ColNum = lines[0].Length;
            Grid = new int[RowNum, ColNum];
            for (int i = 0; i < RowNum; i++)
            {
                for (int j = 0; j < ColNum; j++)
                {
                    Grid[i, j] = int.Parse(lines[i][j].ToString());
                }
            }

            LowPoints = new List<Point>();

            for (int i = 0; i < RowNum; i++)
            {
                for (int j = 0; j < ColNum; j++)
                {
                    if (IsLowPoint(i, j))
                    {
                        LowPoints.Add(new Point(i, j));
                    }
                }
            }
        }

        private bool IsLowPoint(int i, int j)
        {

            return (i == 0 || Grid[i, j] < Grid[i - 1, j]) &&// Check above
                (i == RowNum - 1 || Grid[i, j] < Grid[i + 1, j]) && // Check below
                (j == 0 || Grid[i, j] < Grid[i, j - 1]) && // Check left
                (j == ColNum - 1 || Grid[i, j] < Grid[i, j + 1]); // Check right
        }

        public string[] Lines { get; }
        public int RowNum { get; }
        public int ColNum { get; }
        public int[,] Grid { get; }
        public List<Point> LowPoints { get; }

        public object SolvePart1()
        {
            int sum = 0;
            foreach (var point in LowPoints)
            {
                sum += Grid[point.X, point.Y] + 1;
            }
            return sum;
        }



        public object SolvePart2()
        {
            List<int> flood_sizes = new List<int>();
            foreach (var point in LowPoints)
            {
                flood_sizes.Add(FloodBasin(point));
            }
            flood_sizes.Sort();
            return flood_sizes.OrderByDescending(item => item).Take(3).Aggregate((left, right) => left * right);
        }

        private int FloodBasin(Point point)
        {
            var basin = new HashSet<Point>() { point };
            Queue<Point> points = new Queue<Point>();
            points.Enqueue(point);
            while (points.Count > 0)// Flood Fill
            {
                var expandable = points.Dequeue();
                var i = expandable.X;
                var j = expandable.Y;
                Grid[i, j] = 9;
                if (i > 0 && Grid[i - 1, j] != 9)// UP
                {
                    points.Enqueue(new Point(i - 1, j));
                    basin.Add(new Point(i - 1, j));
                }
                if (i < RowNum - 1 && Grid[i + 1, j] != 9) // DOWN
                {
                    points.Enqueue(new Point(i + 1, j));
                    basin.Add(new Point(i + 1, j));
                }
                if (j > 0 && Grid[i, j - 1] != 9) // LEFT
                {
                    points.Enqueue(new Point(i, j - 1));
                    basin.Add(new Point(i, j - 1));
                }
                if (j < ColNum - 1 && Grid[i, j + 1] != 9) // RIGHT
                {
                    points.Enqueue(new Point(i, j + 1));
                    basin.Add(new Point(i, j + 1));
                }
            }
            return basin.Count;
        }
    }
}
