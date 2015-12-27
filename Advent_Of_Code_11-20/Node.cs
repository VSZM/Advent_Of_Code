using System;
using System.Collections.Generic;

namespace Advent_Of_Code_11_20
{
    internal class Node<TState> : IEquatable<Node<TState>>, IComparable<Node<TState>>
    {
        public TState State { get; set; }
        public Node<TState> Parent { get; set; }
        public Operator<TState> ParentOperator { get; set; }
        public long CostSoFar { get; set; }
        public HashSet<Operator<TState>> OperatorsTried { get; set; }

        public Node(TState state, Node<TState> parent, Operator<TState> parentOperator, long costSoFar)
        {
            State = state;
            OperatorsTried = new HashSet<Operator<TState>>();
            Parent = parent;
            ParentOperator = parentOperator;
            CostSoFar = costSoFar;
        }

        public string Road()
        {
            string parent = Parent == null ? "" : Parent.Road();
            string parent_op = ParentOperator == null ? "" : string.Format("= ({0}) =>", ParentOperator.Cost);
            return parent + parent_op + ToString();
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Node<TState>) obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TState>.Default.GetHashCode(State);
        }

        public bool Equals(Node<TState> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<TState>.Default.Equals(State, other.State);
        }

        public int CompareTo(Node<TState> other)
        {
            return CostSoFar.CompareTo(other.CostSoFar);
        }
    }
}