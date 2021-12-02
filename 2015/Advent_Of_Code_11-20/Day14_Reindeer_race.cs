using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_Of_Code_11_20
{
    public class Day14ReindeerRace : ISolvable
    {
        private class Reindeer
        {
            private string _name;
            // km/h is the first part, duration in seconds is the second
            private KeyValuePair<int, int> _fly;
            private readonly int _rest;
            public int points;

            public Reindeer(string name, int rest, KeyValuePair<int, int> fly)
            {
                _name = name;
                _rest = rest;
                _fly = fly;
            }

            public int Distance_Travelled(int travelDuration)
            {
                int cycle = _rest + _fly.Value;

                return (travelDuration / cycle) * _fly.Value * _fly.Key // Traveling cycle times fly duration times fly speed
                       + Math.Min(travelDuration % cycle, _fly.Value) * _fly.Key;// we travel for the remaining seconds, but no more than the fly duration of a cycle 
            }
        }


        public string Solve(string[] inputLines, bool isPart2)
        {
            List<Reindeer> reindeers = new List<Reindeer>(inputLines.Length);

            reindeers.AddRange(
                inputLines.Select(line => line.Split()).
                    Select(splittedLine => new Reindeer(splittedLine[0],
                        int.Parse(splittedLine[13]),
                        new KeyValuePair<int, int>(int.Parse(splittedLine[3]), int.Parse(splittedLine[6])))));
            
            if (!isPart2)
            {
                return reindeers.ConvertAll(t => t.Distance_Travelled(2503)).Max().ToString();
            }

            for (int second = 1; second <= 2503; ++second)
            {
                int lead_value = reindeers.ConvertAll(t => t.Distance_Travelled(second)).Max();
                var leaders = reindeers.Where(r => r.Distance_Travelled(second) == lead_value);

                foreach (Reindeer reindeer in leaders)
                {
                    reindeer.points++;
                }
            }

            return reindeers.ConvertAll(r => r.points).Max().ToString();
        }
    }
}