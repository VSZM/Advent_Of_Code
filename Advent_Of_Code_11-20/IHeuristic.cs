
namespace Advent_Of_Code_11_20
{
    internal interface IHeuristic<TState>
    {
        int Heuristic_Distance(Node<TState> node, Problem<TState> p);
    }
}