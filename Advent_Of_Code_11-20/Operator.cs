namespace Advent_Of_Code_11_20
{
    internal abstract class Operator<TState> //: IEquatable<Operator<State>> 
    {
        public int Cost { get; }

        protected Operator(int cost)
        {
            Cost = cost;
        }

        //public abstract bool Equals(Operator<State> other);

        public abstract Node<TState> Apply(Node<TState> node);
    }
}