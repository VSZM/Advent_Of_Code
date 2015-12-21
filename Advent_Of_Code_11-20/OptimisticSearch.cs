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

            while (!Problem.Is_Goal_State(act_state))
            {
                Operator oper = Select_Operator(act_state);
                
                if(oper == null) // All operators have been tried out
                    break;

                act_state.OperatorsTried.Add(oper);
                State new_state = act_state.Apply_Operator(act_state, oper);
            }

            if (!Problem.Is_Goal_State(act_state)) return false;
            
            Solution = act_state;
            return true;
        }

    }
}