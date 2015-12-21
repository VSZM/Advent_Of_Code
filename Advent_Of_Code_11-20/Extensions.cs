using System.Text;

namespace Advent_Of_Code_11_20
{
    static class Extensions
    {
        public static string ReplaceAt(this string str, string replacement, int position, int replace_length)
        {
            StringBuilder sb = new StringBuilder(str);
            sb.Remove(position, replace_length);
            sb.Insert(position, replacement);
            return sb.ToString();
        }
    }
}
