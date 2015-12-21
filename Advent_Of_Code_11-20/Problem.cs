using System.Collections.Generic;

namespace Advent_Of_Code_11_20
{
    internal abstract class Problem
    {
        public readonly State StartState;

        protected Problem(State startState)
        {
            StartState = startState;
        }

        public abstract List<Operator> Available_Operators(State state);
        public abstract bool Is_Goal_State(State state);
    }
}