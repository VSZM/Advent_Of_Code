using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AOC2021
{
    class Scanner
    {
        // https://www.reddit.com/r/adventofcode/comments/rjpf7f/comment/hp54e16/?utm_source=share&utm_medium=web2x&context=3
        private static readonly Func<Vector3, float> x = (Vector3 vec3) => vec3.X;
        private static readonly Func<Vector3, float> y = (Vector3 vec3) => vec3.Y;
        private static readonly Func<Vector3, float> z = (Vector3 vec3) => vec3.Z;
        private static readonly List<List<Func<Vector3, float>>> permutations_xzy = new List<List<Func<Vector3, float>>>()
            {
                new List<Func<Vector3, float>>() { x, y, z },
                new List<Func<Vector3, float>>() { x, z, y },
                new List<Func<Vector3, float>>() { y, x, z },
                new List<Func<Vector3, float>>() { y, z, x },
                new List<Func<Vector3, float>>() { z, x, y },
                new List<Func<Vector3, float>>() { z, y, x }
            };
        private static readonly Func<float, float> positive = (float v) => v;
        private static readonly Func<float, float> negative = (float v) => -1 * v;
        private static readonly List<List<Func<float, float>>> permutations_posneg = new List<List<Func<float, float>>>()
            {

                new List<Func<float, float>>(){ positive, positive, positive },
                new List<Func<float, float>>(){ positive, positive, negative },
                new List<Func<float, float>>(){ positive, negative, positive },
                new List<Func<float, float>>(){ positive, negative, negative },
                new List<Func<float, float>>(){ negative, positive, positive },
                new List<Func<float, float>>(){ negative, positive, negative },
                new List<Func<float, float>>(){ negative, negative, positive },
                new List<Func<float, float>>(){ negative, negative, negative }
            };

        private string scanner_name;
        public List<Vector3> beacons;
        internal Vector3 position;

        public Scanner(string scanner_name, List<Vector3> beacons)
        {
            this.scanner_name = scanner_name;
            this.beacons = beacons;
        }

        public IEnumerable<List<Vector3>> GenerateTransformations()
        {
            foreach (var perm_xyz in permutations_xzy)
            {
                foreach (var perm_posneg in permutations_posneg)
                {
                    List<Vector3> transformed_beacons = beacons.Select(b =>
                                        new Vector3(
                                            perm_posneg[0](perm_xyz[0](b)),
                                            perm_posneg[1](perm_xyz[1](b)),
                                            perm_posneg[2](perm_xyz[2](b)))).ToList();
                    //Console.WriteLine("permxyz = {0}, permposneg = {1}, value = {2}", perm_xyz, perm_posneg, transformed_beacons[0]);
                    yield return transformed_beacons;
                }
            }
        }

        public override int GetHashCode()
        {
            return scanner_name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (Scanner)obj;
            if (other == null)
                return false;
            return scanner_name == other.scanner_name;
        }

        public override string ToString()
        {
            return scanner_name;
        }

    }

    internal class Day19 : ISolvable
    {
        private List<Scanner> scanners = new List<Scanner>();

        public Day19(string[] lines)
        {
            var scanner_name = "";
            var transformable_scanners = new Queue<Scanner>();
            var beacons = new List<Vector3>();
            foreach (var line in lines)
            {
                if (line.Contains("scanner"))
                {
                    scanner_name = line.Split(' ')[2];
                }
                else if (line.Length == 0)
                {
                    transformable_scanners.Enqueue(new Scanner(scanner_name, beacons));
                    beacons = new List<Vector3>();
                }
                else
                {
                    var coords = line.Split(',');
                    beacons.Add(new Vector3(int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2])));
                }
            }

            scanners.Add(transformable_scanners.Dequeue());
            scanners[0].position = Vector3.Zero;


            while (transformable_scanners.Count > 0)
            {
                var transformable_scanner = transformable_scanners.Dequeue();
                foreach (var transformed_beacons in transformable_scanner.GenerateTransformations())
                {
                    Vector3? translate_vector = null;
                    Scanner matching_scanner = null;
                    foreach (var anchor_scanner in scanners)
                    {
                        var beacon_set = anchor_scanner.beacons;
                        foreach (var anchor_beacon in beacon_set)
                        {
                            foreach (var beacon in transformed_beacons)
                            {
                                translate_vector = anchor_beacon - beacon;
                                var matches = transformed_beacons.Select(b => b + translate_vector.Value).Where(b => beacon_set.Contains(b));
                                if (matches.Count() >= 12)
                                {
                                    matching_scanner = anchor_scanner;
                                    break;
                                }
                                else
                                {
                                    translate_vector = null;
                                }
                            }
                            if (translate_vector != null)
                                break;

                        }
                        if (translate_vector != null)
                        {

                            break;
                        }
                    }
                    if (translate_vector != null)
                    {
                        transformable_scanner.beacons = transformed_beacons.Select(b => b + translate_vector.Value).ToList();
                        transformable_scanner.position = translate_vector.Value;
                        scanners.Add(transformable_scanner);
                        break;
                    }
                }
                if (!scanners.Contains(transformable_scanner))
                    transformable_scanners.Enqueue(transformable_scanner);
            }

        }

        public object SolvePart1()
        {
            return scanners.SelectMany(s => s.beacons).ToHashSet().Count;
        }

        public object SolvePart2()
        {
            int max_distance = 0;
            foreach (var scanner1 in scanners)
            {
                foreach (var scanner2 in scanners)
                {
                    var distance_vector = scanner1.position - scanner2.position;
                    var distance = Math.Abs(distance_vector.X) + Math.Abs(distance_vector.Y) + Math.Abs(distance_vector.Z); 
                    if(distance > max_distance)
                    {
                        max_distance = (int)distance;
                    }
                }
            }
            return max_distance;
        }
    }
}
