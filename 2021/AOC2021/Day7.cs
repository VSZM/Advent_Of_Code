using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2021
{
    public class Day7 : ISolvable
    {

        public Day7(string[] lines)
        {
            Crabs = lines[0].Split(',').Select(item => int.Parse(item)).ToList();
            CostDict1 = new Dictionary<int, int>();
            CostDict2 = new Dictionary<int, int>();
            // Proof of cost function being monotone
            /*
            for (int i = Crabs.Min(); i <= Crabs.Max(); i++)
            {
                Console.WriteLine(CalculatePositionCost2(i));
            }
            */
        }

        public List<int> Crabs { get; }
        // Memoization
        public Dictionary<int, int> CostDict1 { get; }
        public Dictionary<int, int> CostDict2 { get; }

        private int CalculatePositionCost1(int pos)
        {
            if(CostDict1.ContainsKey(pos)){
                return CostDict1[pos];
            }
            int cost = 0;
            foreach (var crab in Crabs)
            {
                cost += Math.Abs(crab - pos);
            }
            CostDict1[pos] = cost;
            return cost;
        }

        private int CalculatePositionCost2(int pos)
        {
            if (CostDict2.ContainsKey(pos))
            {
                return CostDict2[pos];
            }
            int cost = 0;
            foreach (var crab in Crabs)
            {
                var distance = Math.Abs(crab - pos) + 1;
                cost += distance * (distance - 1) / 2; // N * (N-1) / 2
            }
            CostDict2[pos] = cost;
            return cost;
        }



        public object Solve(Func<int, int> CostFunction)
        {
            var left = Crabs.Min();
            var right = Crabs.Max();

            while (left < right) // Binary search, as the cost function is monotone
            {
                var left_cost = CostFunction(left);
                var right_cost = CostFunction(right);
                var mid1 = (left + right) / 2;
                var mid2 = mid1+1;
                var mid1_cost = CostFunction(mid1);
                var mid2_cost = CostFunction(mid2);
                if(mid1_cost < mid2_cost)
                {
                    right = mid1;
                }
                else
                {
                    left = mid2;
                }
            }
            return CostFunction(left);
        }

        public object SolvePart1()
        {
            return Solve(CalculatePositionCost1);
        }

        public object SolvePart2()
        {
            return Solve(CalculatePositionCost2);
        }
        
    }
}
