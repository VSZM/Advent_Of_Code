using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AOC2021
{

    class Day12 : ISolvable
    {
        public Day12(string[] lines)
        {
            Neighbours = new Dictionary<string, ISet<string>>();
            foreach (var line in lines)
            {
                var left = line.Split('-')[0];
                var right = line.Split('-')[1];
                if (Neighbours.ContainsKey(left))
                {
                    Neighbours[left].Add(right);
                }
                else
                {
                    Neighbours[left] = new HashSet<string>() { right };
                }

                if (Neighbours.ContainsKey(right))
                {
                    Neighbours[right].Add(left);
                }
                else
                {
                    Neighbours[right] = new HashSet<string>() { left };
                }
            }
        }

        public Dictionary<string, ISet<string>> Neighbours { get; }

        public object SolvePart1()
        {
            List<List<string>> start_to_end_routes = new List<List<string>>();
            Queue<List<string>> routes = new Queue<List<string>>();
            routes.Enqueue(new List<string>() { "start" });
            while (routes.Count > 0)
            {
                var current = routes.Dequeue();
                var node = current.Last();
                if (node == "end")
                {
                    start_to_end_routes.Add(current);
                }
                else
                {
                    Neighbours[node].Where(neighbour => char.IsUpper(neighbour[0]) || !current.Contains(neighbour)).ToList().ForEach(neigbour =>
                    {
                        var newroute = new List<string>(current);
                        newroute.Add(neigbour);
                        routes.Enqueue(newroute);
                    });
                }
            }
            return start_to_end_routes.Count;
        }

        public object SolvePart2()
        {
            List<List<string>> start_to_end_routes = new List<List<string>>();
            Queue<List<string>> routes = new Queue<List<string>>();
            routes.Enqueue(new List<string>() { "start" });
            while (routes.Count > 0)
            {
                var current = routes.Dequeue();
                var node = current.Last();
                if (node == "end")
                {
                    start_to_end_routes.Add(current);
                }
                else
                {
                    var lower_nodes_in_path = current.Where(node => node != "start" && char.IsLower(node[0])).ToList();
                    var contains_lower_duplicate = lower_nodes_in_path.Count != new HashSet<string>(lower_nodes_in_path).Count;
                    if (contains_lower_duplicate)
                    {
                        Neighbours[node].Where(neighbour => char.IsUpper(neighbour[0]) || !current.Contains(neighbour)).ToList().ForEach(neigbour =>
                        {
                            var newroute = new List<string>(current);
                            newroute.Add(neigbour);
                            routes.Enqueue(newroute);
                        });
                    } else
                    {
                        Neighbours[node].Where(neighbour => char.IsUpper(neighbour[0]) || neighbour != "start").ToList().ForEach(neigbour =>
                        {
                            var newroute = new List<string>(current);
                            newroute.Add(neigbour);
                            routes.Enqueue(newroute);
                        });
                    }

                }
            }
            return start_to_end_routes.Count;
        }
    }
}
