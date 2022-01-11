using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    internal class Day25 : ISolvable
    {
        public Day25(string[] lines)
        {
            RowCount = lines.Length;
            ColCount = lines[0].Length;
            Grid = new Char[RowCount, ColCount];
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColCount; j++)
                {
                    Grid[i, j] = lines[i][j];
                }
            }
        }

        public char[,] Grid { get; }
        public int RowCount { get; }
        public int ColCount { get; }

        private int FindDeadlock()
        {
            bool deadlock;
            var current = (char[,])Grid.Clone();
            var iters = 0;

            do
            {
                var next = (char[,])current.Clone();
                deadlock = true;

                for (int i = 0; i < RowCount; i++)
                {
                    for (int j = 0; j < ColCount; j++)
                    {
                        if (current[i, j] == '>' && TryMove(i, j, i, j + 1, current, next))
                            deadlock = false;
                    }
                }

                current = next;
                next = (char[,])current.Clone();

                for (int i = 0; i < RowCount; i++)
                {
                    for (int j = 0; j < ColCount; j++)
                    {
                        if (current[i, j] == 'v' && TryMove(i, j, i + 1, j, current, next))
                            deadlock = false;
                    }
                }

                current = next;
                ++iters;
            } while (!deadlock);

            return iters;
        }

        private bool TryMove(int i, int j, int destI, int destJ, char[,] current, char[,] next)
        {
            if (destI == RowCount)
                destI = 0;
            if (destJ == ColCount)
                destJ = 0;

            if (current[destI, destJ] != '.')
                return false;

            next[i, j] = '.';
            next[destI, destJ] = current[i, j];

            return true;
        }

        public object SolvePart1()
        {
            return FindDeadlock();
        }

        public object SolvePart2()
        {
            return "Enjoy the 50 stars you handsome coder!";
        }
    }
}
