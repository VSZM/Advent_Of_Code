using System;

namespace Advent_Of_Code_11_20
{
    internal class Operator : IEquatable<Operator> 
    {
        public State FromState { get; set; }
        public State ToState { get; set; }
        public int Cost { get; set; }

        public bool Equals(Operator other)
        {
            return FromState.Equals(other.FromState) && ToState.Equals(other.ToState) && Cost == other.Cost;
        }
    }
}