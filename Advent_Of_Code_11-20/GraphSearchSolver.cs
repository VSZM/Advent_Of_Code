namespace Advent_Of_Code_11_20
{
    internal abstract class GraphSearchSolver<TState>
    {
        protected Problem<TState> Problem;
        public Node<TState> Solution { get; protected set; }

        protected GraphSearchSolver(Problem<TState> problem)
        {
            Problem = problem;
        }

        protected abstract Operator<TState> Select_Operator(Node<TState> actNode);

        public abstract bool Solve();
    }
}