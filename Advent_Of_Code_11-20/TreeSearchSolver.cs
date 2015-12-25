
using System.Collections.Generic;

namespace Advent_Of_Code_11_20
{
    abstract class TreeSearchSolver<TState>
    {
        public Node<TState> Solution { get; private set; }
        protected Problem<TState> Problem;

        protected TreeSearchSolver(Problem<TState> problem)
        {
            Problem = problem;
        }

        public bool Solve()
        {
            var open_nodes = new List<Node<TState>>() { Problem.StartNode };
            var closed_nodes = new List<Node<TState>>();
            Node<TState> act_node = null;

            while (open_nodes.Count > 0)
            {
                act_node = Select_Node(open_nodes);

                if (Problem.Is_Goal_State(act_node))
                    break;

                Expand_Node(act_node, open_nodes, closed_nodes);
            }

            if (open_nodes.Count == 0)
                return false;

            Solution = act_node;

            return true;
        }

        protected abstract void Expand_Node(Node<TState> node, List<Node<TState>> openNodes, List<Node<TState>> closedNodes);
        protected abstract Node<TState> Select_Node(List<Node<TState>> openNodes);
    }

    class ASearch<TState> : TreeSearchSolver<TState>
    {
        private IHeuristic<TState> _heuristic; 
        public ASearch(Problem<TState> problem, IHeuristic<TState> heuristic)
            : base(problem)
        {
            _heuristic = heuristic;
        }

        protected override void Expand_Node(Node<TState> node, List<Node<TState>> openNodes, List<Node<TState>> closedNodes)
        {
            foreach (var op in Problem.Available_Operators(node))
            {
                var new_node = op.Apply(node);
                var open_idx = openNodes.IndexOf(new_node);
                var closed_idx = closedNodes.IndexOf(new_node);

                if (open_idx == -1 && closed_idx == -1)
                {
                    openNodes.Add(new_node);
                }
                else if (open_idx != -1 && new_node.CostSoFar < openNodes[open_idx].CostSoFar)
                {
                    openNodes[open_idx].CostSoFar = new_node.CostSoFar;
                    openNodes[open_idx].Parent = new_node.Parent;
                    openNodes[open_idx].ParentOperator = new_node.ParentOperator;
                }else if (closed_idx != -1 && new_node.CostSoFar < closedNodes[closed_idx].CostSoFar)
                {
                    closedNodes.RemoveAt(closed_idx);
                    openNodes.Add(new_node);
                }
            }

            openNodes.Remove(node);
            closedNodes.Add(node);
        }

        protected override Node<TState> Select_Node(List<Node<TState>> openNodes)
        {
            int min_idx = 0;

            for (int i = 1; i < openNodes.Count; i++)
            {
                if (openNodes[i].CostSoFar < openNodes[min_idx].CostSoFar)
                    min_idx = i;
            }

            return openNodes[min_idx];
        }
    }

    class OptimalTreeSearchSolver<TState> : TreeSearchSolver<TState>
    {
        public OptimalTreeSearchSolver(Problem<TState> problem)
            : base(problem)
        {
        }

        protected override void Expand_Node(Node<TState> node, List<Node<TState>> openNodes, List<Node<TState>> closedNodes)
        {
            foreach (var op in Problem.Available_Operators(node))
            {
                var new_node = op.Apply(node);
                var open_idx = openNodes.IndexOf(new_node);
                var closed_idx = closedNodes.IndexOf(new_node);

                if (open_idx == -1 && closed_idx == -1)
                {
                    openNodes.Add(new_node);
                }
                else if (open_idx != -1 && new_node.CostSoFar < openNodes[open_idx].CostSoFar)
                {
                    openNodes[open_idx].CostSoFar = new_node.CostSoFar;
                    openNodes[open_idx].Parent = new_node.Parent;
                    openNodes[open_idx].ParentOperator = new_node.ParentOperator;
                }
            }

            openNodes.Remove(node);
            closedNodes.Add(node);
        }

        protected override Node<TState> Select_Node(List<Node<TState>> openNodes)
        {
            int min_idx = 0;

            for (int i = 1; i < openNodes.Count; i++)
            {
                if (openNodes[i].CostSoFar < openNodes[min_idx].CostSoFar)
                    min_idx = i;
            }

            return openNodes[min_idx];
        }
    }
}
