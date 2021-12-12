using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace AOC2021
{
    class Day11 : ISolvable
    {
        public Day11(string[] lines)
        {
            Grid = new int[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Grid[i, j] = int.Parse(lines[i][j].ToString());
                }
            }
            FlashCounter = 0;
        }

        public int[,] Grid { get; }
        public int FlashCounter { get; private set; }

        private void IncrementEnergy(int[,] grid)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    grid[i, j] += 1;
                }
            }
        }

        private void IncrementAdjacentEnergy(int i, int j, int[,] grid)
        {
            List<Point> adjacents = new List<Point>()
            {
                new Point(i-1, j), new Point(i-1,j+1), new Point(i,j+1), new Point(i+1, j+1),
                new Point(i+1, j), new Point(i+1,j-1), new Point(i, j-1), new Point(i-1, j-1)
            };
            adjacents = adjacents.Where(p => p.X >= 0 && p.Y >= 0 && p.X < 10 && p.Y < 10).ToList();
            foreach (var p in adjacents)
            {
                grid[p.X, p.Y]++;
            }
        }

        private int Flash(int[,] grid)
        {
            var has_flashed = new bool[10, 10];
            var flash_this_round = false;
            do
            {
                flash_this_round = false;
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (grid[i, j] > 9 && !has_flashed[i,j])
                        {
                            has_flashed[i, j] = true;
                            IncrementAdjacentEnergy(i, j, grid);
                            flash_this_round = true;
                            FlashCounter++;
                        }
                    }
                }


            } while (flash_this_round);

            var flash_count = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (has_flashed[i, j])
                    {
                        flash_count++;
                        grid[i, j] = 0;
                    }
                }
            }
            return flash_count;
        }



        public object SolvePart1()
        {
            int[,] grid = (int[,])Grid.Clone();
            for (int i = 0; i < 100; i++)
            {
                IncrementEnergy(grid);
                Flash(grid);
            }
            return FlashCounter;
        }

        public object SolvePart2()
        {
            int[,] grid = (int[,])Grid.Clone();
            for (int i = 0; true; i++)
            {
                IncrementEnergy(grid);
                if(Flash(grid) == 100)
                {
                    return i+1;
                }
            }
        }
    }
}
