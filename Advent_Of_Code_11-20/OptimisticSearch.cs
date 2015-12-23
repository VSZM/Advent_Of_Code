namespace Advent_Of_Code_11_20
{
    internal class OptimisticSearch<TState> : HeuristicGraphSearchSolver<TState>
    {
        public OptimisticSearch(Problem<TState> problem, IHeuristic<TState> heuristic)
            : base(problem, heuristic)
        {
        }

        public override bool Solve()
        {
            Node<TState> act_node = Problem.StartNode;

            while (act_node!= null && !Problem.Is_Goal_State(act_node))
            {
                Operator<TState> oper = Select_Operator(act_node);

                if (oper == null) // All operators have been tried out
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
}