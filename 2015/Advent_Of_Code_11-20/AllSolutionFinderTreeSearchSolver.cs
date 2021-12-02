using System;
using System.Collections.Generic;

namespace Advent_Of_Code_11_20
{
    delegate void HandleSolutionFound<TState>(Node<TState> solutionNode);

    abstract class AllSolutionFinderTreeSearchSolver<TState>
    {
        public IReadOnlyList<Node<TState>> Solutions { get { return _solutions; } }
        protected readonly Problem<TState> Problem;
        protected readonly List<Node<TState>> _solutions;
        private readonly HandleSolutionFound<TState> _solutionHandler; 

        protected AllSolutionFinderTreeSearchSolver(Problem<TState> problem, HandleSolutionFound<TState> solutionHandler)
        {
            Problem = problem;
            _solutionHandler = solutionHandler;
            _solutions = new List<Node<TState>>();
        }

        public bool Solve()
        {
            var open_nodes = new List<Node<TState>>() { Problem.StartNode };
            var closed_nodes = new List<Node<TState>>();

            while (open_nodes.Count > 0)
            {
                var act_node = Select_Node(open_nodes);

                if (Problem.Is_Goal_State(act_node))
                {
                    _solutions.Add(act_node);
                    open_nodes.Remove(act_node);
                    _solutionHandler(act_node);
                    continue;
                }

                Expand_Node(act_node, open_nodes, closed_nodes);
            }


            return _solutions.Count > 0;
        }

        protected abstract void Expand_Node(Node<TState> node, List<Node<TState>> openNodes, List<Node<TState>> closedNodes);

        protected abstract Node<TState> Select_Node(List<Node<TState>> openNodes);
    }

    class DFSAllSolutionFinderTreeSearchSolver<TState> : AllSolutionFinderTreeSearchSolver<TState>
    {
        public DFSAllSolutionFinderTreeSearchSolver(Problem<TState> problem, HandleSolutionFound<TState> solutionHandler) : base(problem, solutionHandler)
        {
        }

        protected override void Expand_Node(Node<TState> node, List<Node<TState>> openNodes, List<Node<TState>> closedNodes)
        {
            throw new NotImplementedException();
        }

        protected override Node<TState> Select_Node(List<Node<TState>> openNodes)
        {
            throw new NotImplementedException();
        }
    }
}
