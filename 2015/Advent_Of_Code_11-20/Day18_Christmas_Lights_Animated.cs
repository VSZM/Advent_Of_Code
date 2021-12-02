using System;

namespace Advent_Of_Code_11_20
{
    class Day18ChristmasLightsAnimated : ISolvable
    {
        private static bool Is_In_Range(int[,] grid, int i, int j)
        {
            return i >= 0 && i < grid.GetLength(0) && j >= 0 && j < grid.GetLength(1);
        }

        private static int Get_Next_State(int[,] state, int i, int j)
        {
            int on_count = 0;
            int[] i_neighbour = { -1, -1, -1, 0, 0, +1, +1, +1 };
            int[] j_neighbour = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int k = 0; k < i_neighbour.Length; k++)
            {
                if (!Is_In_Range(state, i + i_neighbour[k], j + j_neighbour[k])) continue;
                if (state[i + i_neighbour[k], j + j_neighbour[k]] == 1)
                    ++on_count;
            }

            if (state[i, j] == 1)
            {
                if (on_count == 2 || on_count == 3)
                    return 1;
                return 0;
            }
            if (on_count == 3)
                return 1;

            return 0;
        }

        private static int[,] Iterate(int[,] state, bool isPart2)
        {
            int[,] next_state = new int[state.GetLength(0), state.GetLength(1)];

            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    next_state[i, j] = Get_Next_State(state, i, j);
                }
            }

            if (isPart2)
            {
                next_state[0, 0] = 1;
                next_state[0, state.GetLength(1) - 1] = 1;
                next_state[state.GetLength(0) - 1, 0] = 1;
                next_state[state.GetLength(0) - 1, state.GetLength(1) - 1] = 1;
            }

            return next_state;
        }

        public string Solve(string[] inputLines, bool isPart2)
        {
            int[,] grid = new int[inputLines.Length, inputLines.Length];

            for (int i = 0; i < inputLines.Length; i++)
            {
                for (int j = 0; j < inputLines[i].Length; j++)
                {
                    grid[i, j] = inputLines[i][j] == '#' ? 1 : 0;
                }
            }

            if (isPart2)
            {
                grid[0, 0] = 1;
                grid[0, grid.GetLength(1) - 1] = 1;
                grid[grid.GetLength(0) - 1, 0] = 1;
                grid[grid.GetLength(0) - 1, grid.GetLength(1) - 1] = 1;
            }

            for (int i = 0; i < 100; i++)
            {
                grid = Iterate(grid, isPart2);
                //      Print_Grid(grid);
            }

            Print_Grid(grid);
            return Grid_On_Value(grid).ToString();
        }

        private static void Print_Grid(int[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    Console.Write(grid[i, j] == 0 ? '.' : '#');
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        private static int Grid_On_Value(int[,] grid)
        {
            int sum = 0;
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    sum += grid[i, j];
                }
            }

            return sum;
        }
    }
}
