using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_Of_Code_11_20
{
    class Day11Hacking : ISolvable
    {
        public static string Increment_String(string str)
        {
            var charstr = str.ToCharArray();

            int increment_idx = charstr.Length - 1;
            bool turnover_happened;

            do
            {
                turnover_happened = false;

                if (charstr[increment_idx] == 'z')
                {
                    charstr[increment_idx] = 'a';
                    if (increment_idx == 0)
                    // if we reached the beginning of the string, we need to append a new char at the start
                    {

                        charstr = ("a" + new string(charstr)).ToCharArray();
                        break;
                    }
                    turnover_happened = true;
                }
                else
                {
                    charstr[increment_idx] = (char)(charstr[increment_idx] + 1);
                }

                --increment_idx;
            } while (turnover_happened);

            return new string(charstr);
        }


        public string Solve(string[] inputLines, bool is_part_2)
        {
            while (!Is_Valid_Password(inputLines[0]))
                inputLines[0] = Increment_String(inputLines[0]);

            return inputLines[0];
        }

        public static bool Is_Valid_Password(string inputLine)
        {
            bool contains_3_increasing = false;
            HashSet<char> excluded = new HashSet<char> { 'i', 'o', 'l' };

            if (inputLine.Any(t => excluded.Contains(t)))
            {
                return false;
            }

            for (int i = 0; i < inputLine.Length - 2; ++i)
                if (inputLine[i + 1] == inputLine[i] + 1 && inputLine[i + 2] == inputLine[i + 1] + 1)
                {
                    contains_3_increasing = true;
                    break;
                }
            HashSet<char> pair_chars = new HashSet<char>();


            for (int i = 0; i < inputLine.Length - 1; ++i)
            {
                if (inputLine[i] == inputLine[i + 1] && !pair_chars.Contains(inputLine[i]))
                {
                    pair_chars.Add(inputLine[i]);
                    ++i;
                }
            }

            return contains_3_increasing && pair_chars.Count >= 2;
        }
	}
}
