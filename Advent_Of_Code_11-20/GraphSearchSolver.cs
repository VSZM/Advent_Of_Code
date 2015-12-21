namespace Advent_Of_Code_11_20
{
    internal abstract class GraphSearchSolver
    {
        protected Problem Problem;
        public State Solution { get; set; }

        protected GraphSearchSolver(Problem problem)
        {
            Problem = problem;
        }

        protected abstract Operator Select_Operator(State actState);

        public abstract bool Solve();
    }
}