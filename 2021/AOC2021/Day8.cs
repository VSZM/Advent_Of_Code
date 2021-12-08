using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC2021
{

    class SevenSegmentDisplay
    {
        /*
                                     
                 TOP        -->      aaaa
                 TOPLEFT    -->     b    c   <-- TOPRIGHT
                                    b    c
                 MIDDLE     -->      dddd
                 BOTTOMLEFT -->     e    f   <-- BOTTOMRIGHT
                                    e    f
                 BOTTOM     -->      gggg
                            
        */

        /* Each SET contains the candidate characters for that position */
        public ISet<char> Top { get; }
        public ISet<char> Bottom { get; }
        public ISet<char> Middle { get; }
        public ISet<char> Topleft { get; }
        public ISet<char> Topright { get; }
        public ISet<char> Bottomleft { get; }
        public ISet<char> Bottomright { get; }
        public List<ISet<char>> AllSegments { get; }
        public List<ISet<char>> Zero { get; }
        public List<ISet<char>> One { get; }
        public List<ISet<char>> Two { get; }
        public List<ISet<char>> Three { get; }
        public List<ISet<char>> Four { get; }
        public List<ISet<char>> Five { get; }
        public List<ISet<char>> Six { get; }
        public List<ISet<char>> Seven { get; }
        public List<ISet<char>> Eight { get; }
        public List<ISet<char>> Nine { get; }

        public SevenSegmentDisplay()
        {
            Top = new HashSet<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };
            Bottom = new HashSet<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };
            Middle = new HashSet<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };
            Topleft = new HashSet<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };
            Topright = new HashSet<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };
            Bottomleft = new HashSet<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };
            Bottomright = new HashSet<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };
            AllSegments = new List<ISet<char>>() { Top, Bottom, Middle, Topleft, Topright, Bottomleft, Bottomright };
            Zero = new List<ISet<char>>() { Top, Bottom, Topleft, Topright, Bottomleft, Bottomright };
            One = new List<ISet<char>>() { Topright, Bottomright };
            Two = new List<ISet<char>>() { Top, Bottom, Middle, Topright, Bottomleft, };
            Three = new List<ISet<char>>() { Top, Bottom, Middle, Topright, Bottomright };
            Four = new List<ISet<char>>() { Middle, Topleft, Topright, Bottomright };
            Five = new List<ISet<char>>() { Top, Bottom, Middle, Topleft, Bottomright };
            Six = new List<ISet<char>>() { Top, Bottom, Middle, Topleft, Bottomleft, Bottomright };
            Seven = new List<ISet<char>>() { Top, Topright, Bottomright };
            Eight = new List<ISet<char>>() { Top, Bottom, Middle, Topleft, Topright, Bottomleft, Bottomright };
            Nine = new List<ISet<char>>() { Top, Bottom, Middle, Topleft, Topright, Bottomright };
        }

        public int GetDigit(string wire_state)
        {
            foreach (var segment in AllSegments)
            {
                if (segment.Count != 1)
                {
                    throw new InvalidOperationException();
                }
            }
            var on_segments = AllSegments.Where(segment => wire_state.Contains(segment.First())).ToList();
            if (on_segments.SequenceEqual(Zero))
                return 0;
            if (on_segments.SequenceEqual(One))
                return 1;
            if (on_segments.SequenceEqual(Two))
                return 2;
            if (on_segments.SequenceEqual(Three))
                return 3;
            if (on_segments.SequenceEqual(Four))
                return 4;
            if (on_segments.SequenceEqual(Five))
                return 5;
            if (on_segments.SequenceEqual(Six))
                return 6;
            if (on_segments.SequenceEqual(Seven))
                return 7;
            if (on_segments.SequenceEqual(Eight))
                return 8;
            if (on_segments.SequenceEqual(Nine))
                return 9;


            throw new InvalidOperationException();
        }

        public int GetNumber(string[] digit_wire_states)
        {
            int number = 0;
            foreach (var wire_state in digit_wire_states)
            {
                number *= 10;
                number += GetDigit(wire_state);
            }
            return number;
        }
    }

    class Day8 : ISolvable
    {
        public Day8(string[] lines)
        {
            Outputs = lines.Select(line => line.Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToList();
            Patterns = lines.Select(line => line.Split('|')[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToList();
        }

        public string[] Lines { get; }
        public List<string[]> Outputs { get; }
        public List<string[]> Patterns { get; }

        public object SolvePart1()
        {
            var count_1748 = 0;
            foreach (var outputs in Outputs)
            {
                count_1748 += outputs.Select(str => str.Length).Count(length => length == 2 || length == 3 || length == 4 || length == 7);
            }
            return count_1748;
        }

        public object SolvePart2()
        {
            var sum = 0;
            foreach (var pair in Enumerable.Zip(Patterns, Outputs))
            {
                var display = new SevenSegmentDisplay();
                var one_pattern = pair.First.Where(str => str.Length == 2).First().ToCharArray().ToHashSet();
                var seven_pattern = pair.First.Where(str => str.Length == 3).First().ToCharArray().ToHashSet();
                var four_pattern = pair.First.Where(str => str.Length == 4).First().ToCharArray().ToHashSet();
                var eight_pattern = pair.First.Where(str => str.Length == 7).First().ToCharArray().ToHashSet(); // contains all chars
                var six_pattern = pair.First.Where(str => str.Length == 6 && one_pattern.Except(str.ToCharArray()).Count() > 0).First().ToCharArray().ToHashSet();
                var zero_pattern = pair.First.Where(str => str.Length == 6 && one_pattern.Except(str.ToCharArray()).Count() == 0 
                                                        && four_pattern.Except(str.ToCharArray()).Count() > 0).First().ToCharArray().ToHashSet();
                var nine_pattern = pair.First.Where(str => str.Length == 6 && one_pattern.Except(str.ToCharArray()).Count() == 0
                                                        && four_pattern.Except(str.ToCharArray()).Count() == 0).First().ToCharArray().ToHashSet();

                // Remove characters in 1 from segments not used in 1
                var should_not_lit = display.AllSegments.Except(display.One).ToList();
                should_not_lit.ForEach(segment => segment.ExceptWith(one_pattern));
                // Remove characters not in 1 from segments used in 1
                display.One.ForEach(segment => segment.ExceptWith(eight_pattern.Except(one_pattern)));

                // Remove characters in 7 from segments not used in 7
                should_not_lit = display.AllSegments.Except(display.Seven).ToList();
                should_not_lit.ForEach(segment => segment.ExceptWith(seven_pattern));
                // Remove characters not in 7 from segments used in 7
                display.Seven.ForEach(segment => segment.ExceptWith(eight_pattern.Except(seven_pattern)));

                // Remove characters in 4 from segments not used in 4
                should_not_lit = display.AllSegments.Except(display.Four).ToList();
                should_not_lit.ForEach(segment => segment.ExceptWith(four_pattern));
                // Remove characters not in 4 from segments used in 4
                display.Four.ForEach(segment => segment.ExceptWith(eight_pattern.Except(four_pattern)));


                // Deduce right side based on diff between 1 and 6
                display.Bottomright.ExceptWith(one_pattern.Except(six_pattern));
                display.Topright.ExceptWith(display.Bottomright);

                // Deduce mid based on diff between 0 and 8
                display.Topleft.ExceptWith(eight_pattern.Except(zero_pattern));
                display.Middle.ExceptWith(display.Topleft);

                // Deduce bottom-left based on diff between 9 and 8
                display.Bottom.ExceptWith(eight_pattern.Except(nine_pattern));
                display.Bottomleft.ExceptWith(display.Bottom);


                sum += display.GetNumber(pair.Second);
            }
            return sum;
        }
    }
}
