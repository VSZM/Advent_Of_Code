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
            var base_formula = inputLines.Last();
            _transitions =
                new HashSet<KeyValuePair<string, string>>(inputLines.Where(line => line.Contains("=>")).Select(line =>
                {
                    var splitted_line = line.Split();
                    return new KeyValuePair<string, string>(splitted_line[0], splitted_line[2]);
                }));


            if (isPart2)
            {
                GraphSearchSolver solver = new OptimisticSearch(new MoleculeProblem(new MoleculeState(base_formula, null, null, 0), "e",_transitions), new MoleculeHeuristic());
                if (solver.Solve())
                    return solver.Solution.CostSoFar.ToString();
                return "-1";
            }
            foreach (var transition in _transitions)
            {
                for (var pos = 0; pos < base_formula.Length; pos++)
                {
                    var index = base_formula.IndexOf(transition.Key, pos, StringComparison.CurrentCulture);
                    if (index == -1)
                        break;
                    _medicines.Add(base_formula.ReplaceAt(transition.Value, index, transition.Key.Length));
                    pos = index;
                }
            }

            return _medicines.Count.ToString();
        }
    }

    internal class MoleculeProblem : Problem
    {
        public string Goal { get; }
        private ISet<KeyValuePair<string, string>> _transitions;


        public MoleculeProblem(State startState, string goal, ISet<KeyValuePair<string, string>> transitions) : base(startState)
        {
            Goal = goal;
            _transitions = transitions;
        }

        public override List<Operator> Available_Operators(State state)
        {
            List<Operator> ret = new List<Operator>();

            MoleculeState ms = state as MoleculeState;
            if (ms == null || ms.Molecule.Length < Goal.Length)
                return ret;

            foreach (var transition in _transitions)
            {
                for (var pos = 0; pos < ms.Molecule.Length; pos++)
                {
                    var index = ms.Molecule.IndexOf(transition.Value, pos, StringComparison.CurrentCulture);
                    if (index == -1)
                        break;
                    ret.Add(new MoleculeOperator(transition.Value, transition.Key, index));
                    pos = index;
                }
            }

            return ret;
        }

        public override bool Is_Goal_State(State state)
        {
            MoleculeState ms = state as MoleculeState;
            if (ms == null)
                return false;

            return Goal == ms.Molecule;
        }
    }

    internal class MoleculeHeuristic : IHeuristic
    {
        public int Heuristic_Distance(State state, Problem p)
        {
            MoleculeProblem mp = p as MoleculeProblem;
            MoleculeState ms = state as MoleculeState;

            if (ms == null || mp == null)
                throw new ArgumentException();

            return mp.Goal.Distance(ms.Molecule);
        }
    }

    internal class MoleculeOperator : Operator
    {
        public string From { get; }
        public string To { get; }
        public int Position { get; }

        public MoleculeOperator(string from, string to, int position) : base(1)
        {
            From = from;
            To = to;
            Position = position;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MoleculeOperator)obj);
        }

        protected bool Equals(MoleculeOperator other)
        {
            return string.Equals(From, other.From) && string.Equals(To, other.To) && Position == other.Position;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (From != null ? From.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (To != null ? To.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Position;
                return hashCode;
            }
        }

        public override bool Equals(Operator other)
        {
            var o = other as MoleculeOperator;
            if (o == null)
                return false;

            return o.From == From && o.To == To && o.Position == Position;
        }

        public override State Apply(State state)
        {
            var base_state = state as MoleculeState;

            if (base_state == null)
                return null;

            return new MoleculeState(base_state.Molecule.ReplaceAt(To, Position, From.Length), base_state, this, Cost + base_state.CostSoFar);
        }
    }

    internal class MoleculeState : State
    {
        public string Molecule { get; }

        public MoleculeState(string molecule, State parent, Operator parentOperator, int costSoFar)
            : base(parent, parentOperator, costSoFar)
        {
            Molecule = molecule;
        }

        public override string ToString()
        {
            return Molecule;
        }

        protected bool Equals(MoleculeState other)
        {
            return string.Equals(Molecule, other.Molecule);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MoleculeState)obj);
        }

        public override int GetHashCode()
        {
            return (Molecule != null ? Molecule.GetHashCode() : 0);
        }

        public override bool Equals(State other)
        {
            var o = other as MoleculeState;
            if (o != null)
            {
                return o.Molecule == Molecule;
            }
            return false;
        }
    }
}
