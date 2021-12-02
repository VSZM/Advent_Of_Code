using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

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

        public virtual bool Solve()
        {
            Node<TState> act_node = Problem.StartNode;

            while (act_node != null && !Problem.Is_Goal_State(act_node))
            {
                Operator<TState> oper = Select_Operator(act_node);

                if (oper == null) // All operators have been tried out OR no operators ovailable from node
                {
                    act_node = act_node.Parent;
                    continue;
                }

                act_node.OperatorsTried.Add(oper);

                Node<TState> new_node = oper.Apply(act_node);
                act_node = new_node;
            }

            if (!Problem.Is_Goal_State(act_node)) return false;

            Solution = act_node;
            return true;
        }
    }


    internal class BestSolutionLimitedGraphSearchSolver<TState> : GraphSearchSolver<TState>
    {
        private readonly Random rand = new Random();
        private readonly int _maxCost;
        private readonly int _maxIter;

        public BestSolutionLimitedGraphSearchSolver(Problem<TState> problem, int max_cost = Int32.MaxValue, int max_iter = Int32.MaxValue) : base(problem)
        {
            _maxCost = max_cost;
            _maxIter = max_iter;
        }

        public override bool Solve()
        {
            Node<TState> act_node = Problem.StartNode;
            int iter = 0;

            while (act_node != null && iter++ < _maxIter)
            {
                if (act_node.CostSoFar > _maxCost)
                {
                    act_node = act_node.Parent;
                    continue;
                }

                if (Problem.Is_Goal_State(act_node))
                {
                    if (Solution == null || act_node.CostSoFar < Solution.CostSoFar)
                    {
                        Solution = act_node;
                        Console.WriteLine("Found a new solution! Cost: " + Solution.CostSoFar);
                    }
                    act_node = act_node.Parent;
                    continue;
                }

                Operator<TState> oper = Select_Operator(act_node);

                if (oper == null) // All operators have been tried out OR no operators ovailable from node
                {
                    act_node = act_node.Parent;
                    continue;
                }

                act_node.OperatorsTried.Add(oper);

                Node<TState> new_node = oper.Apply(act_node);
                act_node = new_node;
            }

            return Solution != null;
        }

        protected override Operator<TState> Select_Operator(Node<TState> actNode)
        {
            var available_operators = Problem.Available_Operators(actNode).Where(oper => !actNode.OperatorsTried.Contains(oper)).ToList();

            return available_operators.Count == 0 ? null : available_operators[0];
        }
    }

    internal class HeuristicGraphSearchSolver<TState> : GraphSearchSolver<TState>
    {
        protected IHeuristic<TState> Heuristic;

        public HeuristicGraphSearchSolver(Problem<TState> problem, IHeuristic<TState> heuristic)
            : base(problem)
        {
            Heuristic = heuristic;
        }

        protected override Operator<TState> Select_Operator(Node<TState> actNode)// Selecting te best operator which leads to the best heuristic distance
        {
            List<Operator<TState>> avalible_operators =
                Problem.Available_Operators(actNode)
                    .Where(oper => !actNode.OperatorsTried.Contains(oper))
                    .ToList();

            return avalible_operators.Count == 0 ? null : avalible_operators.OrderBy(oper => Heuristic.Heuristic_Distance(oper.Apply(actNode), Problem)).First();
        }
    }

}