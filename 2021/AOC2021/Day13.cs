using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AOC2021
{
    public class Day13 : ISolvable
    {
        private const string IMAGE_NAME = "Day13_part2.bmp";

        public Day13(string[] lines)
        {
            var coordinates = lines.Where(line => line.Contains(','))
                                   .Select(line => new Point(int.Parse(line.Split(',')[0]), int.Parse(line.Split(',')[1])))
                                   .ToList();
            MaxX = coordinates.Select(point => point.X).Max() + 2;
            MaxY = coordinates.Select(point => point.Y).Max() + 2;
            Grid = new bool[MaxY, MaxX];
            foreach (var coord in coordinates)
            {
                Grid[coord.Y, coord.X] = true;
            }
            Folds = new Queue<string>();
            lines.Where(line => line.Contains("fold")).ToList().ForEach(line => Folds.Enqueue(line));
            DoFold();
        }

        private void DoFold()
        {
            var fold = Folds.Dequeue();
            var value = int.Parse(fold.Split('=')[1]);
            if (fold.Contains('x'))
            {
                for (int i = 1; value - i >= 0; i++)
                {
                    for (int j = 0; j < MaxY; j++)
                    {
                        Grid[j, value - i] |= Grid[j, value + i];
                    }
                }
                MaxX = value;
                var new_grid = new bool[MaxY, MaxX];
                for (int i = 0; i < MaxX; i++)
                {
                    for (int j = 0; j < MaxY; j++)
                    {
                        new_grid[j, i] = Grid[j, i];
                    }
                }
                Grid = new_grid;
            }
            else
            {
                for (int j = 1; value - j >= 0; j++)
                {
                    for (int i = 0; i < MaxX; i++)
                    {
                        Grid[value - j, i] |= Grid[value + j, i];
                    }
                }
                MaxY = value;
                var new_grid = new bool[MaxY, MaxX];
                for (int i = 0; i < MaxX; i++)
                {
                    for (int j = 0; j < MaxY; j++)
                    {
                        new_grid[j, i] = Grid[j, i];
                    }
                }
                Grid = new_grid;
            }
        }

        private void Print()
        {
            for (int j = 0; j < MaxY; j++)
            {
                for (int i = 0; i < MaxX; i++)
                {
                    if (Grid[j, i]) Console.Write('#');
                    else Console.Write('.');
                }
                Console.WriteLine();
            }
        }

        private void Draw()
        {
            var bmp = new Bitmap(MaxX, MaxY);
            for (int i = 0; i < MaxX; i++)
            {
                for (int j = 0; j < MaxY; j++)
                {
                    bmp.SetPixel(i, j, Grid[j,i] ? Color.White : Color.Black);
                }
            }
            bmp.Save(IMAGE_NAME);
        }

        private int DotCount()
        {
            var count = 0;
            for (int j = 0; j < MaxY; j++)
            {
                for (int i = 0; i < MaxX; i++)
                {
                    if (Grid[j, i]) count++;
                }
            }
            return count;
        }


        public bool[,] Grid { get; set; }
        public Queue<string> Folds { get; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }

        public object SolvePart1()
        {
            //Print();
            return DotCount();
        }

        public object SolvePart2()
        {
            while(Folds.Count > 0)
            {
                DoFold();
            }

            Draw();
            return "See " + IMAGE_NAME;
        }
    }
}
