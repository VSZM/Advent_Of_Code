using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    public class Day24 : ISolvable
    {
        public Day24(string[] lines)
        {
        }

        static int[] div = new int[14] { 1, 1, 1, 1, 1, 26, 26, 1, 26, 1, 26, 26, 26, 26 };
        static int[] check = new int[14] { 10, 10, 14, 11, 14, -14, 0, 10, -10, 13, -12, -3, -11, -2 };
        static int[] offset = new int[14] { 2, 4, 8, 7, 12, 7, 10, 14, 2, 6, 8, 11, 5, 11 };


        // These are the states will always produce false
        static HashSet<(int depth, int z)> SURELY_BAD_STATES = new HashSet<(int depth, int z)>();
        

        private long? GenerateModelNumber(int depth, long modelNumber, int z, int[] digits)
        {
            if (SURELY_BAD_STATES.Contains((depth, z)) || depth == 14)
                return null;

            modelNumber *= 10;
            var originalZ = z;

            for (int i = 0; i < digits.Length; ++i)
            {
                z = originalZ;
                int w = digits[i];
                var x = z;
                x %= 26;
                z /= div[depth];
                x += check[depth];
                x = x == w ? 1 : 0;
                x = x == 0 ? 1 : 0;
                var y = 25;
                y *= x;
                y += 1;
                z *= y;
                y = w;
                y += offset[depth];
                y *= x;
                z += y;

                if (z == 0 && depth == 13)
                    return modelNumber + digits[i];
                var ret = GenerateModelNumber(depth + 1, modelNumber + digits[i], z, digits);
                if(ret != null)
                    return ret;

            }

            SURELY_BAD_STATES.Add((depth, originalZ));

            return null;
        }

        public object SolvePart1()
        {
            return GenerateModelNumber(0, 0, 0, new int[] { 9, 8, 7, 6, 5, 4, 3, 2, 1});
        }

        public object SolvePart2()
        {
            return GenerateModelNumber(0, 0, 0, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9});
        }
    }
}

