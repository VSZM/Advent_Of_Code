using System.Collections.Generic;
using System.Linq;

namespace Advent_Of_Code_11_20
{
    internal abstract class HeuristicGraphSearchSolver : GraphSearchSolver 
    {
        protected IHeuristic Heuristic;

        protected HeuristicGraphSearchSolver(Problem problem, IHeuristic heuristic)
            : base(problem)
        {
            Heuristic = heuristic;
        }

        protected override Operator Select_Operator(State actState)// Selecting te best operator which leads to the best heuristic distance
        {
            List<Operator> avalible_operators =
                Problem.Available_Operators(actState)
                    .Where(oper => !(actState.OperatorsTried as IEnumerable<Operator>).Contains(oper))
                    .ToList();

            return avalible_operators.Count == 0 ? null : avalible_operators.OrderBy(oper => Heuristic.Heuristic_Distance(oper.ToState, Problem)).First();
        }
    }
}