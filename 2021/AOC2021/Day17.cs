using System;
using System.Drawing;

namespace AOC2021
{
    public class Day17 : ISolvable
    {
        public Day17(string[] lines)
        {
            // target area: x=20..30, y=-10..-5
            var line = lines[0].Substring(12);
            var parts = line.Split(',');
            Xmin = Convert.ToInt32(parts[0].Split("..")[0].Substring(3));
            Xmax = Convert.ToInt32(parts[0].Split("..")[1]);
            Ymin = Convert.ToInt32(parts[1].Split("..")[0].Substring(3));
            Ymax = Convert.ToInt32(parts[1].Split("..")[1]);
            Solve();
        }

        public int Xmin { get; }
        public int Xmax { get; }
        public int Ymin { get; }
        public int Ymax { get; }
        public int YReach { get; set; }
        public int ReachCount { get; private set; }

        private bool IsInside(Point p)
        {
            return p.X >= Xmin && p.X <= Xmax && p.Y >= Ymin && p.Y <= Ymax;
        }

        private bool IsVectorReaching(Point velocity, out int max_y)
        {
            Point pos = new Point(0, 0);
            max_y = pos.Y;

            while (pos.Y > Ymin || pos.Y > 0)
            {
                pos.X += velocity.X;
                pos.Y += velocity.Y;
                velocity.X += Math.Sign(velocity.X) * -1;
                velocity.Y -= 1;
                if (pos.Y > max_y)
                {
                    max_y = pos.Y;
                }
                if (IsInside(pos))
                {
                    return true;
                }
            }

            return false;
        }

        private void Solve()
        {
            YReach = int.MinValue;
            int no_reach_since = 0;
            for (int y = Ymin; no_reach_since < 20; y++)
            {
                for (int x = 1; x <= Xmax; x++)
                {
                    int max_y;
                    if (IsVectorReaching(new Point(x, y), out max_y))
                    {
                        if (max_y > YReach)
                        {
                            YReach = max_y;
                            Console.WriteLine("New Height ({2}) Found with initial V({0}, {1})", x, y, YReach);
                        }
                        ReachCount++;
                        no_reach_since = 0;
                    }
                }
                no_reach_since++;
            }
        }

        public object SolvePart1()
        {
            return YReach;
        }

        public object SolvePart2()
        {
            return ReachCount;
        }
    }
}

