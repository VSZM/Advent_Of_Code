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


        private bool Monad(long modelnumber)
        {
            var inputs = modelnumber.ToString().Reverse().Select(x => int.Parse(x.ToString())).ToList();
            var div = new int[14] { 1, 1, 1, 1, 1, 26, 26, 1, 26, 1, 26, 26, 26, 26 };
            var check = new int[14] { 10, 10, 14, 11, 14, -14, 0, 10, -10, 13, -12, -3, -11, -2 };
            var offset = new int[14] { 2, 4, 8, 7, 12, 7, 10, 14, 2, 6, 8, 11, 5, 11 };

            int x = 0;
            int y = 0;
            int z = 0;


            for (int i = 0; i < 14; i++)
            {
                int w = inputs[i];
                x = z; // 0 
                x %= 26; //0
                z /= div[i]; // 0
                x += check[i]; // x = 10
                x = x == w ? 1 : 0; // x = 0
                x = x == 0 ? 1 : 0; // x = 1
                if (x != 0)
                {
                    y = 25;
                    y *= x; // y = 25
                    y += 1; // y = 26
                    z *= y; // z = 0
                    y = w;
                    y += offset[i]; // y = w + 2
                    y *= x;// THIS RIGHT here! If x == 0 then this entire iteration is NOOP
                    z += y;
                }
            }
            return z == 0;
        }


        long GenerateNumber()
        {
            Random random = new Random();
            long ret;
            do
            {
                ret = random.NextInt64(11111111111111L, 99999999999999L);
            } while (ret.ToString().Contains('0'));
            return ret;
        }

        private long GreatestModelNumber()
        {
            long max = long.MinValue;
            object mutex = new object();
            Console.WriteLine("Progress: {0} %", 0);
            Parallel.For(0L, 88888888888888L + 1L, new ParallelOptions { MaxDegreeOfParallelism = 32 }, (i, pls) =>
            {
                var number = 99999999999999L - i;
                if (number.ToString().Contains('0'))
                    return;
                if (Monad(number))
                {
                    Console.WriteLine("Found Model number! {0}", number);
                    lock (mutex)
                    {
                        if (number > max)
                        {
                            max = number;
                            Console.WriteLine("New record model number found! {0}", number);
                            pls.Break();
                        }
                    }
                }
            });

            return max;
        }

        public object SolvePart1()
        {
            return GreatestModelNumber();
        }

        public object SolvePart2()
        {
            throw new NotImplementedException();
        }
    }
}

