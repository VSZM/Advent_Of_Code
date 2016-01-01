
using System;
using System.Collections.Generic;

namespace Advent_Of_Code_11_20
{
    internal abstract class TreeSearchSolver<TState>
    {
        public Node<TState> Solution { get; private set; }
        protected Problem<TState> Problem;

        protected TreeSearchSolver(Problem<TState> problem)
        {
            Problem = problem;
        }

        public bool Solve()
        {
            var open_nodes = new LinkedList<Node<TState>>();
            open_nodes.AddFirst(Problem.StartNode);
            var closed_nodes = new LinkedList<Node<TState>>();
            LinkedListNode<Node<TState>> act_node = null;
            long highest_cost_so_far = int.MinValue;

            while (open_nodes.Count > 0)
            {
                act_node = Select_Node(open_nodes);

                if (act_node.Value.CostSoFar > highest_cost_so_far)
                {
                    Console.WriteLine("{0} Reached cost: {1} open nodes: {2}", DateTime.Now, act_node.Value.CostSoFar, open_nodes.Count);
                    highest_cost_so_far = act_node.Value.CostSoFar;
                }

                if (Problem.Is_Goal_State(act_node.Value))
                    break;

                Expand_Node(act_node, open_nodes, closed_nodes);
            }

            if (open_nodes.Count == 0)
                return false;

            Solution = act_node.Value;

            return true;
        }

        protected abstract void Expand_Node(LinkedListNode<Node<TState>> node, LinkedList<Node<TState>> openNodes, LinkedList<Node<TState>> closedNodes);
        protected abstract LinkedListNode<Node<TState>> Select_Node(LinkedList<Node<TState>> openNodes);
    }

    internal class ASearch<TState> : TreeSearchSolver<TState>
    {
        private IHeuristic<TState> _heuristic;
        public ASearch(Problem<TState> problem, IHeuristic<TState> heuristic)
            : base(problem)
        {
            _heuristic = heuristic;
        }

        protected override void Expand_Node(LinkedListNode<Node<TState>> node, LinkedList<Node<TState>> openNodes, LinkedList<Node<TState>> closedNodes)
        {
            foreach (var op in Problem.Available_Operators(node.Value))
            {
                var new_node = op.Apply(node.Value);
                var open_node = openNodes.Find(new_node);
                var closed_node = closedNodes.Find(new_node);

                if (open_node == null && closed_node == null)
                {
                    openNodes.AddLast(new_node);
                }
                else if (open_node != null && new_node.CostSoFar < open_node.Value.CostSoFar)
                {
                    open_node.Value.CostSoFar = new_node.CostSoFar;
                    open_node.Value.Parent = new_node.Parent;
                    open_node.Value.ParentOperator = new_node.ParentOperator;
                }
                else if (closed_node != null && new_node.CostSoFar < closed_node.Value.CostSoFar)
                {
                    closedNodes.Remove(closed_node);
                    openNodes.AddLast(closed_node);
                }
            }

            openNodes.Remove(node);
            closedNodes.AddLast(node);
        }

        protected override LinkedListNode<Node<TState>> Select_Node(LinkedList<Node<TState>> openNodes)
        {
            var min_node = openNodes.First;
            var act_node = min_node.Next;
            float min_value = min_node.Value.CostSoFar + _heuristic.Heuristic_Distance(min_node.Value, Problem);


            while (act_node != null)
            {
                float act_value = act_node.Value.CostSoFar + _heuristic.Heuristic_Distance(act_node.Value, Problem);
                if (act_value < min_value)
                {
                    min_node = act_node;
                    min_value = act_value;
                }

                act_node = act_node.Next;
            }

            return min_node;
        }
    }

    internal class DfsTreeSearchSolver<TState> : TreeSearchSolver<TState>
    {
        public DfsTreeSearchSolver(Problem<TState> problem) : base(problem)
        {
        }

        protected override void Expand_Node(LinkedListNode<Node<TState>> node, LinkedList<Node<TState>> openNodes, LinkedList<Node<TState>> closedNodes)
        {
            foreach (var op in Problem.Available_Operators(node.Value))
            {
                var new_node = op.Apply(node.Value);
                var open_node = openNodes.Find(new_node);
                var closed_node = closedNodes.Find(new_node);

                if (open_node == null && closed_node == null)
                {
                    openNodes.AddLast(new_node);
                }
            }

            openNodes.Remove(node);
            closedNodes.AddLast(node);
        }

        protected override LinkedListNode<Node<TState>> Select_Node(LinkedList<Node<TState>> openNodes)
        {
            return openNodes.Last;
        }
    }

    internal class BfsTreeSearchSolver<TState> : TreeSearchSolver<TState>
    {
        private readonly bool _checkCycles;

        public BfsTreeSearchSolver(Problem<TState> problem, bool checkCycles = true) : base(problem)
        {
            _checkCycles = checkCycles;
        }

        protected override void Expand_Node(LinkedListNode<Node<TState>> node, LinkedList<Node<TState>> openNodes, LinkedList<Node<TState>> closedNodes)
        {
            foreach (var op in Problem.Available_Operators(node.Value))
            {
                var new_node = op.Apply(node.Value);
                if (_checkCycles)
                {
                    var open_node = openNodes.Find(new_node);
                    var closed_node = closedNodes.Find(new_node);

                    if (open_node == null && closed_node == null)
                    {
                        openNodes.AddLast(new_node);
                    }
                }
                else
                    openNodes.AddLast(new_node);
            }

            openNodes.Remove(node);
            if (_checkCycles)
                closedNodes.AddLast(node);
        }

        protected override LinkedListNode<Node<TState>> Select_Node(LinkedList<Node<TState>> openNodes)
        {
            return openNodes.First;
        }
    }

    internal class OptimalTreeSearchSolver<TState> : TreeSearchSolver<TState>
    {
        private readonly bool _checkCycles;
        public OptimalTreeSearchSolver(Problem<TState> problem, bool checkCycles)
            : base(problem)
        {
            _checkCycles = checkCycles;
        }

        protected override void Expand_Node(LinkedListNode<Node<TState>> node, LinkedList<Node<TState>> openNodes, LinkedList<Node<TState>> closedNodes)
        {
            foreach (var op in Problem.Available_Operators(node.Value))
            {
                var new_node = op.Apply(node.Value);
                if (_checkCycles)
                {
                    var open_node = openNodes.Find(new_node);
                    var closed_node = closedNodes.Find(new_node);

                    if (open_node == null && closed_node == null)
                    {
                        Insert_Cost_Sorted(new_node, openNodes);
                    }
                    else if (open_node != null && new_node.CostSoFar < open_node.Value.CostSoFar)
                    {
                        open_node.Value.CostSoFar = new_node.CostSoFar;
                        open_node.Value.Parent = new_node.Parent;
                        open_node.Value.ParentOperator = new_node.ParentOperator;
                    }
                }
                else
                    Insert_Cost_Sorted(new_node, openNodes);
            }

            openNodes.Remove(node);
            if (_checkCycles) closedNodes.AddLast(node);
        }

        private void Insert_Cost_Sorted(Node<TState> node, LinkedList<Node<TState>> openNodes)
        {
            long diff_from_first = Math.Abs(node.CostSoFar - openNodes.First.Value.CostSoFar);
            long diff_from_last = Math.Abs(node.CostSoFar - openNodes.Last.Value.CostSoFar);

            if(diff_from_last > diff_from_first)
                Insert_Cost_Sorted_From_Beginning(node, openNodes);
            else
                Insert_Cost_Sorted_From_End(node, openNodes);
        }

        private void Insert_Cost_Sorted_From_End(Node<TState> node, LinkedList<Node<TState>> openNodes)
        {
            var act_node = openNodes.Last;

            while (act_node != null && act_node.Value.CostSoFar > node.CostSoFar)
            {
                act_node = act_node.Previous;
            }


            if (act_node == null)
            {
                openNodes.AddFirst(node);
                return;
            }

            openNodes.AddAfter(act_node, node);
        }

        private void Insert_Cost_Sorted_From_Beginning(Node<TState> node, LinkedList<Node<TState>> openNodes)
        {
            var act_node = openNodes.First;

            while (act_node != null && act_node.Value.CostSoFar < node.CostSoFar)
            {
                act_node = act_node.Next;
            }


            if (act_node == null)
            {
                openNodes.AddLast(node);
                return;
            }

            openNodes.AddBefore(act_node, node);
        }

        protected override LinkedListNode<Node<TState>> Select_Node(LinkedList<Node<TState>> openNodes)
        {
            return openNodes.First;
        }
    }
}
