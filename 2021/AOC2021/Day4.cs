using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    class Day4 : ISolvable
    {
        public Day4(string[] lines)
        {
            Lines = lines;
            Matrices = new List<int[,]>();
            Numbers = lines[0].Split(',').Select(str => int.Parse(str));
            int i = 2;
            while (i < lines.Length)
            {
                var m = new int[5, 5];
                for (int row = 0; row < 5; row++, i++)
                {
                    var rownums = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(str => int.Parse(str)).ToArray();
                    for (int col = 0; col < 5; col++)
                    {
                        m[row, col] = rownums[col];
                    }
                }
                Matrices.Add(m);
                i++;
            }

        }

        public string[] Lines { get; }
        public List<int[,]> Matrices { get; }
        public IEnumerable<int> Numbers { get; private set; }

        private void Eliminate_Number_In_Matrix(int[,] m, int number)
        {
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (m[row, col] == number)
                    {
                        m[row, col] = -1;
                    }
                }
            }
        }

        private bool Board_Wins(int[,] m)
        {
            for (int i = 0; i < 5; i++)
            {
                if (Enumerable.Range(0, 5).Select(col => m[i, col]).Sum() == -5)
                {
                    return true;
                }
                if (Enumerable.Range(0, 5).Select(row => m[row, i]).Sum() == -5)
                {
                    return true;
                }
            }
            return false;
        }

        private int Board_Score(int[,] m)
        {
            int sum = 0;
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (m[row, col] > 0)
                    {
                        sum += m[row, col];
                    }
                }
            }
            return sum;
        }

        public object solve_part_1()
        {
            foreach (var number in Numbers)
            {
                foreach (var m in Matrices)
                {
                    Eliminate_Number_In_Matrix(m, number);
                    if (Board_Wins(m))
                    {
                        return Board_Score(m) * number;
                    }
                }
            }
            return -1;
        }

        public object solve_part_2()
        {
            int result = -1;
            foreach (var number in Numbers)
            {
                for (int i = 0; i < Matrices.Count; i++)
                {
                    var m = Matrices[i];
                    Eliminate_Number_In_Matrix(m, number);
                    if (Board_Wins(m))
                    {
                        result = Board_Score(m) * number;
                        Matrices.Remove(m);
                        i--;
                    }
                }
                if(Matrices.Count == 0)
                {
                    break;
                }
            }
            return result;
        }
    }
}
