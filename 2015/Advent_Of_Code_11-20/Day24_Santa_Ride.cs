using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security;

namespace Advent_Of_Code_11_20
{
   /* 
    Tried to solve it with a graph search algo. Turns out building the search tree is very slow.

    internal class SleighProblem : Problem<SleighState>
    {
        private readonly int _targetWeight;
        private readonly HashSet<int> _weights;

        public SleighProblem(int targetWeight, IEnumerable<int> weights, Node<SleighState> startNode) : base(startNode)
        {
            _targetWeight = targetWeight;
            _weights = new HashSet<int>(weights);
        }

        public override List<Operator<SleighState>> Available_Operators(Node<SleighState> node)
        {
            List<Operator<SleighState>> ret = new List<Operator<SleighState>>();

            if (node.CostSoFar >= _targetWeight)
                return ret;

            ret.AddRange(_weights.Where(weight => !node.State._chosenPackages.Contains(weight))
                    .Select(weight => new SleighOperator(weight)));

            return ret;
        }

        public override bool Is_Goal_State(Node<SleighState> node)
        {
            return node.State._chosenPackages.Sum() == _targetWeight;
        }
    }

    internal class SleighOperator : Operator<SleighState>
    {
        private readonly int _addedWeight;

        public SleighOperator(int addedWeight) : base(0)
        {
            _addedWeight = addedWeight;
        }

        public override Node<SleighState> Apply(Node<SleighState> node)
        {
            HashSet<int> new_packages = new HashSet<int>(node.State._chosenPackages) { _addedWeight };

            return new Node<SleighState>(new SleighState(new_packages), node, this, new_packages.Count);
        }
    }

    internal class SleighState : IEquatable<SleighState>, IComparable<SleighState>
    {
        public HashSet<int> _chosenPackages;

        public SleighState() : this(new HashSet<int>())
        {
        }

        public SleighState(HashSet<int> chosenPackages)
        {
            _chosenPackages = chosenPackages;
        }

        public bool Equals(SleighState other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _chosenPackages.Equals(other._chosenPackages);
        }

        public int CompareTo(SleighState other)
        {
            if (_chosenPackages.Count == other._chosenPackages.Count)
                return
                    _chosenPackages.Aggregate((product, actual) => product * actual)
                        .CompareTo(other._chosenPackages.Aggregate((product, actual) => product * actual));

            return _chosenPackages.Count.CompareTo(other._chosenPackages.Count);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SleighState)obj);
        }

        public override int GetHashCode()
        {
            return _chosenPackages.GetHashCode();
        }
    }
    */
    internal class Day24SantaRide : ISolvable
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool Remaining_Packages_Are_Dividable(List<int> weights, int mask, int targetWeight, bool isPart2)
        {

            for (int mask2 = 1; mask2 < Math.Pow(2, weights.Count); mask2++)
            {
                if ((mask2 & mask) > 0)
                    continue;
                int sum = 0;
                for (int i = 0; i < weights.Count; i++)
                {
                    if ((1 << i & mask2) == 0) continue;

                    sum += weights[i];
                }

                if (sum == targetWeight)
                    if (isPart2)
                        return Remaining_Packages_Are_Dividable(weights, mask | mask2, targetWeight, false);
                return true;
            }

            return false;
        }

        public string Solve(string[] inputLines, bool isPart2)
        {
            var weights = inputLines.Select(int.Parse).ToList();
            weights.Reverse();
            int target_weight = weights.Sum() / (isPart2 ? 4 : 3);
            int best_count = int.MaxValue;
            BigInteger corresponding_product = 0L;

            for (int mask = 1; mask < Math.Pow(2, weights.Count); mask++)
            {
                int sum = 0;
                int count = 0;
                BigInteger product = 1;
                for (int i = 0; i < weights.Count; i++)
                {
                    if ((1 << i & mask) == 0) continue;

                    sum += weights[i];
                    product *= weights[i];
                    ++count;
                }

                if (sum == target_weight && (count < best_count || count == best_count && product < corresponding_product) && Remaining_Packages_Are_Dividable(weights, mask, target_weight, isPart2))
                {
                    //          Console.WriteLine("New solution found! {0}, product = {1}", Solution_ToString(weights, mask), product);
                    best_count = count;
                    corresponding_product = product;
                }
            }


            return corresponding_product.ToString();
        }

        private object Solution_ToString(List<int> weights, int mask)
        {
            List<int> ret = weights.Where((t, i) => (1 << i & mask) != 0).ToList();

            return "{" + string.Join(", ", ret) + "}";
        }
    }
}
