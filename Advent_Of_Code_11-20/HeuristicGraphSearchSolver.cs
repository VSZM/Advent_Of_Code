using System.Collections.Generic;
using System.Linq;

namespace Advent_Of_Code_11_20
{
    internal abstract class HeuristicGraphSearchSolver<TState> : GraphSearchSolver<TState>
    {
        protected IHeuristic<TState> Heuristic;

        protected HeuristicGraphSearchSolver(Problem<TState> problem, IHeuristic<TState> heuristic)
            : base(problem)
        {
            Heuristic = heuristic;
        }

        protected override Operator<TState> Select_Operator(Node<TState> actNode)// Selecting te best operator which leads to the best heuristic distance
        {
            List<Operator<TState>> avalible_operators =
                Problem.Available_Operators(actNode)
                    .Where(oper => !(actNode.OperatorsTried as IEnumerable<Operator<TState>>).Contains(oper))
                    .ToList();

            return avalible_operators.Count == 0 ? null : avalible_operators.OrderBy(oper => Heuristic.Heuristic_Distance(oper.Apply(actNode), Problem)).First();
        }
    }
}