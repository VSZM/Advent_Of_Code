using System;
using System.Collections.Generic;

namespace Advent_Of_Code_11_20
{
    internal abstract class State :IEquatable<State>
    {
        public State Parent { get; set; }
        public Operator ParentOperator { get; set; }
        public int CostSoFar { get; set; }
        public HashSet<Operator> OperatorsTried { get; set; }

        protected State(State parent, Operator parentOperator, int costSoFar)
        {
            OperatorsTried = new HashSet<Operator>();
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
        public abstract bool Equals(State other);
    }
}