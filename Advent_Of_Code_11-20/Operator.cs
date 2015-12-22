using System;

namespace Advent_Of_Code_11_20
{
    internal abstract class Operator : IEquatable<Operator> 
    {
        public int Cost { get; }

        public Operator(int cost)
        {
            Cost = cost;
        }

        public abstract bool Equals(Operator other);

        public abstract State Apply(State state);
    }
}