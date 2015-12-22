namespace Advent_Of_Code_11_20
{
    internal class OptimisticSearch : HeuristicGraphSearchSolver 
    {
        public OptimisticSearch(Problem problem, IHeuristic heuristic)
            : base(problem, heuristic)
        {
        }

        public override bool Solve()
        {
            State act_state = Problem.StartState;

            while (act_state!= null && !Problem.Is_Goal_State(act_state))
            {
                Operator oper = Select_Operator(act_state);

                if (oper == null) // All operators have been tried out
                {
                    act_state = act_state.Parent;
                    continue;
                }

                act_state.OperatorsTried.Add(oper);
                
                State new_state = oper.Apply(act_state);
                act_state = new_state;
            }

            if (!Problem.Is_Goal_State(act_state)) return false;
            
            Solution = act_state;
            return true;
        }

    }
}