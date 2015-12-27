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
                GraphSearchSolver<string> solver = new OptimisticSearch<string>(new MoleculeProblem(new Node<string>(base_formula, null, null, 0), "e",_transitions), new MoleculeHeuristic());
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

    internal class MoleculeProblem : Problem<string>
    {
        public string Goal { get; }
        private ISet<KeyValuePair<string, string>> _transitions;


        public MoleculeProblem(Node<string> startNode, string goal, ISet<KeyValuePair<string, string>> transitions) : base(startNode)
        {
            Goal = goal;
            _transitions = transitions;
        }

        public override List<Operator<string>> Available_Operators(Node<string> node)
        {
            List<Operator<string>> ret = new List<Operator<string>>();

            if (node.State.Length < Goal.Length)
                return ret;

            foreach (var transition in _transitions)
            {
                for (var pos = 0; pos < node.State.Length; pos++)
                {
                    var index = node.State.IndexOf(transition.Value, pos, StringComparison.CurrentCulture);
                    if (index == -1)
                        break;
                    ret.Add(new MoleculeOperator(transition.Value, transition.Key, index));
                    pos = index;
                }
            }

            return ret;
        }

        public override bool Is_Goal_State(Node<string> node)
        {
            return Goal == node.State;
        }
    }

    internal class MoleculeHeuristic : IHeuristic<string>
    {
        public float Heuristic_Distance(Node<string> node, Problem<string> p)
        {
            MoleculeProblem mp = p as MoleculeProblem;

            return mp.Goal.Distance(node.State);
        }
    }

    internal class MoleculeOperator : Operator<string>
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

        //public override bool Equals(Operator<string> other)
        //{
        //    var o = other as MoleculeOperator;
        //    if (o == null)
        //        return false;

        //    return false;

        //    return o.From == From && o.To == To && o.Position == Position;
        //}

        public override Node<string> Apply(Node<string> node)
        {
            return new Node<string>(node.State.ReplaceAt(To, Position, From.Length), node, this, Cost + node.CostSoFar);
        }
    }
}
