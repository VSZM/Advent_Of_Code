using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AOC2021
{
    class Instruction
    {
        public int id;
        public bool is_on;
        public int x_from;
        public int x_to;
        public int y_from;
        public int y_to;
        public int z_from;
        public int z_to;

        public Instruction(int id, bool is_on, int x_from, int x_to, int y_from, int y_to, int z_from, int z_to)
        {
            this.id = id;
            this.is_on = is_on;
            this.x_from = x_from;
            this.x_to = x_to;
            this.y_from = y_from;
            this.y_to = y_to;
            this.z_from = z_from;
            this.z_to = z_to;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (Instruction)obj;
            if (other != null)
                return other.id == this.id;
            return false;
        }

    }

    internal class Day22 : ISolvable
    {
        private readonly List<Instruction> instructions = new List<Instruction>();

        public Day22(string[] lines)
        {
            int id = 0;
            foreach (var line in lines)
            {
                var parts = line.Replace(',', ' ').Replace('=', ' ').Split(' ');
                var is_on = "on" == parts[0];
                var x_coords = parts[2].Split("..");
                var y_coords = parts[4].Split("..");
                var z_coords = parts[6].Split("..");
                instructions.Add(new Instruction(id++, is_on, int.Parse(x_coords[0]), int.Parse(x_coords[1]), int.Parse(y_coords[0]), int.Parse(y_coords[1]), int.Parse(z_coords[0]), int.Parse(z_coords[1])));
            }
        }

        private bool Contains(Instruction instruction, (int x, int y, int z) point)
        {
            return instruction.x_from <= point.x && instruction.x_to >= point.x &&
                instruction.y_from <= point.y && instruction.y_to >= point.y &&
                instruction.z_from <= point.z && instruction.z_to >= point.z;
        }

        private long SolveSlow(List<Instruction> instructions)
        {
            int minX = instructions.Where(i => i.is_on).Select(i => Math.Min(i.x_from, i.x_to)).Min();
            int minY = instructions.Where(i => i.is_on).Select(i => Math.Min(i.y_from, i.y_to)).Min();
            int minZ = instructions.Where(i => i.is_on).Select(i => Math.Min(i.z_from, i.z_to)).Min();
            int maxX = instructions.Where(i => i.is_on).Select(i => Math.Max(i.x_from, i.x_to)).Max();
            int maxY = instructions.Where(i => i.is_on).Select(i => Math.Max(i.y_from, i.y_to)).Max();
            int maxZ = instructions.Where(i => i.is_on).Select(i => Math.Max(i.z_from, i.z_to)).Max();

            long count = 0;

            var ons = instructions.Where(i => i.is_on).ToList();
            var offs = instructions.Where(i => !i.is_on).ToList();

            using (var pb = new ProgressBar(maxX - minX, "% Progress"))
            {
                Parallel.For(minX, maxX + 1, new ParallelOptions { MaxDegreeOfParallelism = 128 }, x =>
                {
                    pb.Tick();
                    for (int y = minY; y <= maxY; y++)
                    {
                        for (int z = minZ; z <= maxZ; z++)
                        {
                            var containers = instructions.Where(i => Contains(i, (x, y, z)));
                            if (containers.Count() == 0 || !containers.Last().is_on)
                            {
                                continue;
                            }
                            Interlocked.Increment(ref count);
                        }
                    }
                });
            }
            return count;
        }


        // https://stackoverflow.com/a/66370641/1564252
        private BigInteger Solve(List<Instruction> instructions)
        {
            BigInteger count = 0;
            // These are the boundaries on each axis
            var xs = instructions.SelectMany(instruction => new List<int>() { instruction.x_from })
                                    .Concat(instructions.SelectMany(instruction => new List<int>() { instruction.x_to })).Distinct().OrderBy(x => x).ToList();
            var ys = instructions.SelectMany(instruction => new List<int>() { instruction.y_from })
                                    .Concat(instructions.SelectMany(instruction => new List<int>() { instruction.y_to })).Distinct().OrderBy(y => y).ToList();
            var zs = instructions.SelectMany(instruction => new List<int>() { instruction.z_from })
                                    .Concat(instructions.SelectMany(instruction => new List<int>() { instruction.z_to })).Distinct().OrderBy(z => z).ToList();

            // This generates the intervals. This is needed as we need to make sure each voxel is only checked once, so overlaps are not checked twice. 
            var generate_pairs = (List<int> boundaries) =>
            {
                var pairs = new List<(int x_start, int x_end)>();
                pairs.Add((boundaries[0], boundaries[0]));
                for (int i = 1; i < boundaries.Count; i++)
                {
                    pairs.Add((boundaries[i], boundaries[i]));
                    var start = boundaries[i - 1] + 1;
                    var end = boundaries[i] - 1;
                    if (end - start >= 0)
                    {
                        pairs.Add((start, end));
                    }
                }
                return pairs;
            };
            var xpairs = generate_pairs(xs);
            var ypairs = generate_pairs(ys);
            var zpairs = generate_pairs(zs);


            using (var pb = new ProgressBar(xpairs.Count, "% Progress"))
            {
                foreach (var (x_start, x_end) in xpairs)
                {
                    pb.Tick();
                    var xbox = instructions.Where(instruction => (x_start >= instruction.x_from && x_start <= instruction.x_to)
                                                                        && (x_end >= instruction.x_from && x_end <= instruction.x_to)).ToHashSet();

                    foreach (var (y_start, y_end) in ypairs)
                    {
                        var ybox = instructions.Where(instruction => (y_start >= instruction.y_from && y_start <= instruction.y_to)
                                                                        && (y_end >= instruction.y_from && y_end <= instruction.y_to)).ToHashSet();

                        foreach (var (z_start, z_end) in zpairs)
                        {
                            var zbox = instructions.Where(instruction => (z_start >= instruction.z_from && z_start <= instruction.z_to)
                                                                        && (z_end >= instruction.z_from && z_end <= instruction.z_to)).ToHashSet();

                            var intersection_of_limits = xbox.Intersect(ybox).Intersect(zbox).ToList();
                            if (intersection_of_limits.Count() == 0)
                                continue;

                            var last_instruction_of_intersection = intersection_of_limits.OrderByDescending(instruction => instructions.IndexOf(instruction)).First();
                            if (last_instruction_of_intersection.is_on)
                            {
                                count += (x_end - x_start + 1) * (y_end - y_start + 1) * (z_end - z_start + 1);
                            }
                        }
                    }
                }
            }

            return count;
        }

        public object SolvePart1()
        {
            var initialization_steps = instructions.Where(instruction => instruction.x_from >= -50 && instruction.x_to <= 50 &&
                                                  instruction.y_from >= -50 && instruction.y_to <= 50 &&
                                                  instruction.z_from >= -50 && instruction.z_to <= 50).ToList();
            return Solve(initialization_steps);
        }

        public object SolvePart2()
        {
            return (Solve(instructions), SolveSlow(instructions));
        }
    }
}
