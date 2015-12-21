using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_Of_Code_11_20
{


    internal class Day19Molecules : ISolvable
    {
        private ISet<KeyValuePair<string, string>> _transitions;
        private readonly ISet<string> _medicines = new HashSet<string>();

        public string Solve(string[] inputLines, bool isPart2)
        {
            string base_formula = inputLines.Last();
            _transitions =
                new HashSet<KeyValuePair<string, string>>(inputLines.Where(line => line.Contains("=>")).Select(line =>
                {
                    var splitted_line = line.Split();
                    return new KeyValuePair<string, string>(splitted_line[0], splitted_line[2]);
                }));


            if (isPart2)
            {
                GraphSearchSolver solver = new OptimisticSearch().Solve();
                return solver.Solution.CostSoFar.ToString();
            }
            foreach (KeyValuePair<string, string> transition in _transitions)
            {
                for (int pos = 0; pos < base_formula.Length; pos++)
                {
                    int index = base_formula.IndexOf(transition.Key, pos, StringComparison.CurrentCulture);
                    if (index == -1)
                        break;
                    _medicines.Add(base_formula.ReplaceAt(transition.Value, index, transition.Key.Length));
                    pos = index;
                }
            }

            return _medicines.Count.ToString();
        }
    }

    class MoleculeState : State
    {
        private readonly string _molecule;

        public MoleculeState(string molecule,State parent, Operator parentOperator, int costSoFar)
            : base(parent, parentOperator, costSoFar)
        {
            _molecule = molecule;
        }

        public override State Apply_Operator(State baseState, Operator oper)
        {
            return new MoleculeState(baseState, oper, baseState.CostSoFar + oper.Cost);
        }

        public override string ToString()
        {
            return _molecule;
        }

        public override bool Equals(State other)
        {
            MoleculeState o = other as MoleculeState;
            if (o != null)
            {
                return o._molecule == _molecule;
            }
            return false;
        }
    }
}
