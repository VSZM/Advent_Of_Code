using System.Collections.Generic;

namespace Advent_Of_Code_11_20
{
    internal abstract class Problem<TState>
    {
        public readonly Node<TState> StartNode;

        protected Problem(Node<TState> startNode)
        {
            StartNode = startNode;
        }

        public abstract List<Operator<TState>> Available_Operators(Node<TState> node);
        public abstract bool Is_Goal_State(Node<TState> node);
    }
}