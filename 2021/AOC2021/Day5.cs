using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    class Line : IEnumerable<Point>
    {
        public int StartX { get; private set; }
        public int StartY { get; private set; }
        public int EndX { get; private set; }
        public int EndY { get; private set; }

        public bool IsHorizontal { get { return StartY == EndY; } }
        public bool IsVertical { get { return StartX == EndX; } }

        public bool IsSlant { get { return !IsHorizontal && !IsVertical; } }


        public Line(string line)
        {
            string[] parts = line.Split(" -> ");
            string[] parts_start = parts[0].Split(',');
            string[] parts_end = parts[1].Split(',');
            StartX = int.Parse(parts_start[0]);
            StartY = int.Parse(parts_start[1]);
            EndX = int.Parse(parts_end[0]);
            EndY = int.Parse(parts_end[1]);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}) -> ({2}, {3})", StartX, StartY, EndX, EndY);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            if (IsHorizontal)
            {
                var direction = Math.Sign(EndX - StartX);
                foreach (var point in EnumerableUtilities.RangePython(StartX, EndX + direction, direction).Select(x => new Point(x, StartY)))
                {
                    yield return point;
                }
            }
            else if (IsVertical)
            {
                var direction = Math.Sign(EndY - StartY);
                foreach (var point in EnumerableUtilities.RangePython(StartY, EndY + direction, direction).Select(y => new Point(StartX, y)))
                {
                    yield return point;
                }
            }
            else
            {
                Point current = new Point(StartX, StartY);
                yield return current;
                Point goal = new Point(EndX, EndY);
                int x_dir = Math.Sign(EndX - StartX);
                int y_dir = Math.Sign(EndY - StartY);
                while (current != goal)
                {
                    current = new Point(current.X + x_dir, current.Y + y_dir);
                    yield return current;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class Day5 : ISolvable
    {
        public Day5(string[] lines)
        {
            Lines = lines.Select(line => new Line(line)).ToList();
            MaxXY = Lines.Select(line => new List<int>() { line.StartX, line.StartY, line.EndX, line.EndY }.Max()).Max() + 1;
        }

        public List<Line> Lines { get; }
        public int MaxXY { get; }

        /*private void show_map(int[,] map)
        {
            var img = new Image();

            Bitmap bitmap;
            unsafe
            {
                fixed (int* intPtr = &map[0, 0])
                {
                    bitmap = new Bitmap(width, height, stride, PixelFormat.Format32bppRgb, new IntPtr(intPtr));
                }
            }
        }*/



        public object solve(bool skip_diagonal)
        {
            var map = new int[MaxXY, MaxXY];
            foreach (var line in Lines)
            {
                if (skip_diagonal && line.IsSlant)
                    continue;

                foreach (var point in line)
                {
                    map[point.X, point.Y]++;
                }
            }

            var at_least_2 = 0;
            for (int y = 0; y < MaxXY; y++)
            {
                var str = "";
                for (int x = 0; x < MaxXY; x++)
                {
                    if (map[x, y] >= 2)
                    {
                        at_least_2++;
                    }
                    str += map[x, y];
                }
            }
            //show_map(map);
            return at_least_2;
        }

        public object SolvePart1()
        {
            return solve(true);
        }

        public object SolvePart2()
        {
            return solve(false);
        }
    }
}
