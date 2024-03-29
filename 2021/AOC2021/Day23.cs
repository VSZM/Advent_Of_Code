﻿using ShellProgressBar;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{

    class State : ICloneable
    {
        public int roomCapacity = 2;
        public int costSoFar = 0;
        public Stack<char> roomA = new Stack<char>();
        public Stack<char> roomB = new Stack<char>();
        public Stack<char> roomC = new Stack<char>();
        public Stack<char> roomD = new Stack<char>();
        public Dictionary<char, Stack<char>> rooms = new Dictionary<char, Stack<char>>();
        public State parent = null;

        private string repr = null;

        // 11 slots
        public string hallway = "...........";
        public State()
        {
            rooms['A'] = roomA;
            rooms['B'] = roomB;
            rooms['C'] = roomC;
            rooms['D'] = roomD;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj.ToString() == this.ToString();
        }


        /**
            #############
            #...........#
            ###B#C#B#D###
              #A#D#C#A#
              #########
         * 
         * */

        public override string ToString()
        {
            if (this.repr == null)
            {
                StringBuilder sb = new StringBuilder();
                var aList = string.Join("", roomA.ToList()).PadLeft(roomCapacity, '.');
                var bList = string.Join("", roomB.ToList()).PadLeft(roomCapacity, '.');
                var cList = string.Join("", roomC.ToList()).PadLeft(roomCapacity, '.');
                var dList = string.Join("", roomD.ToList()).PadLeft(roomCapacity, '.');

                sb.Append(new string('#', hallway.Length + 2)).AppendLine();
                sb.Append('#').Append(hallway).Append('#').AppendLine();
                for (int i = 0; i < roomCapacity; i++)
                {
                    sb.Append("###").Append(aList.ToList()[i]).Append('#').Append(bList.ToList()[i]).Append('#').Append(cList.ToList()[i]).Append('#').Append(dList.ToList()[i]).Append("###").AppendLine();

                }
                sb.Append("  ").Append(new string('#', 9)).Append("  ").AppendLine();

                repr = sb.ToString();
            }
            return repr;
        }

        internal bool IsEnd()
        {
            if (rooms.Keys.Select(room => rooms[room].Count(pod => pod != room)).Sum() == 0 && hallway.Count(c => c != '.') == 0)
                return true;
            return false;
        }



        private static List<int> HALLWAY_STOPS = new List<int>() { 0, 1, 3, 5, 7, 9, 10 };

        private static bool IsInvalidState(State state)
        {
            var hallway_count = state.hallway.Count(c => c != '.');
            var acount = state.hallway.Count(c => c == 'A') + state.rooms.Keys.Select(roomkey => state.rooms[roomkey].Count(c => c == 'A')).Sum();
            var bcount = state.hallway.Count(c => c == 'B') + state.rooms.Keys.Select(roomkey => state.rooms[roomkey].Count(c => c == 'B')).Sum();
            var ccount = state.hallway.Count(c => c == 'C') + state.rooms.Keys.Select(roomkey => state.rooms[roomkey].Count(c => c == 'C')).Sum();
            var dcount = state.hallway.Count(c => c == 'D') + state.rooms.Keys.Select(roomkey => state.rooms[roomkey].Count(c => c == 'D')).Sum();

            return hallway_count + state.rooms.Keys.Select(roomkey => state.rooms[roomkey].Count).Sum() != state.roomCapacity * 4
                || acount != state.roomCapacity || bcount != state.roomCapacity || ccount != state.roomCapacity || dcount != state.roomCapacity;
        }

        internal List<State> GetNextPossibleStates()
        {
            var next_states = new List<State>();
            for (int i = 0; i < hallway.Length; i++)// Going from Hallway to one of the rooms
            {
                var pod = hallway[i];
                if (pod != '.' && rooms[pod].Count < roomCapacity && IsNotBlocked(i, Day23.ROOM_INDEX[pod]) && !IsInvaded(pod))
                {
                    var room_distance = Math.Abs(Day23.ROOM_INDEX[pod] - i);
                    var room_entry_cost = roomCapacity - rooms[pod].Count;
                    var new_state = (State)this.Clone();
                    var step_cost = Day23.STEP_COST[pod];
                    new_state.costSoFar += (room_distance + room_entry_cost) * step_cost;
                    var new_hallway = hallway.ToCharArray();
                    new_hallway[i] = '.';
                    new_state.hallway = new string(new_hallway);
                    new_state.rooms[pod].Push(pod);
                    new_state.parent = this;
                    if (!PathContainsState(new_state))
                        next_states.Add(new_state);
                }
            }

            foreach (var roomkey in rooms.Keys)// Going from one of the rooms to a hallway stop
            {
                if (rooms[roomkey].Count == 0 || (rooms[roomkey].Count == roomCapacity && !rooms[roomkey].Where(pod => pod != roomkey).Any()))//room empty or full
                    continue;
                var room_index = Day23.ROOM_INDEX[roomkey];
                var pod = rooms[roomkey].Peek();
                var step_cost = Day23.STEP_COST[pod];
                var room_exit_cost = roomCapacity - rooms[roomkey].Count + 1;
                foreach (var stop in HALLWAY_STOPS.Where(stop => IsNotBlocked(room_index, stop)))
                {
                    var stop_distance = Math.Abs(room_index - stop);
                    var new_state = (State)this.Clone();
                    new_state.costSoFar += (stop_distance + room_exit_cost) * step_cost;
                    new_state.rooms[roomkey].Pop();
                    var new_hallway = hallway.ToCharArray();
                    new_hallway[stop] = pod;
                    new_state.hallway = new string(new_hallway);
                    new_state.parent = this;
                    if (!PathContainsState(new_state))
                        next_states.Add(new_state);
                }
            }

            return next_states;
        }

        private bool PathContainsState(State new_state)
        {
            State current = this;
            while (current != null)
            {
                if (current == new_state)
                    return true;
                current = current.parent;
            }
            return false;
        }

        private bool IsInvaded(char pod)
        {
            return rooms[pod].Where(ch => ch != pod).Any();
        }

        private bool IsNotBlocked(int from, int to)
        {
            var dir = Math.Sign(to - from);
            do
            {
                from += dir;
                if (hallway[from] != '.')
                    return false;
            } while (from != to);

            return true;
        }

        public object Clone()
        {
            var new_state = new State();
            new_state.roomCapacity = roomCapacity;
            new_state.costSoFar = costSoFar;
            new_state.roomA = new Stack<char>(roomA.Reverse());
            new_state.roomB = new Stack<char>(roomB.Reverse());
            new_state.roomC = new Stack<char>(roomC.Reverse());
            new_state.roomD = new Stack<char>(roomD.Reverse());
            new_state.rooms['A'] = new_state.roomA;
            new_state.rooms['B'] = new_state.roomB;
            new_state.rooms['C'] = new_state.roomC;
            new_state.rooms['D'] = new_state.roomD;
            new_state.hallway = new string(hallway);
            return new_state;
        }

        public void PrintPath()
        {
            State current = this;
            Stack<State> path = new Stack<State>();
            do
            {
                path.Push(current);
                current = current.parent;
            } while (current != null);

            while (path.Count > 0)
            {
                current = path.Pop();
                Console.WriteLine(current);
                Console.WriteLine(current.costSoFar);
                Console.WriteLine(new String('=', 20) + ">");
            }
        }
    }

    internal class Day23 : ISolvable
    {

        /*
           
            Test:
            #############
            #...........#
            ###B#C#B#D###
              #A#D#C#A#
              #########
        
            Input:
            #############
            #...........#
            ###D#A#D#C###
              #B#C#B#A#
              #########
         
         
         */

        public Day23(string[] lines)
        {
            Start = new State();
            Start.roomA.Push(lines[3][3]);
            Start.roomA.Push(lines[2][3]);
            Start.roomB.Push(lines[3][5]);
            Start.roomB.Push(lines[2][5]);
            Start.roomC.Push(lines[3][7]);
            Start.roomC.Push(lines[2][7]);
            Start.roomD.Push(lines[3][9]);
            Start.roomD.Push(lines[2][9]);
        }

        public State Start { get; }


        private State SolveShortestPathSearch(State start)
        {
            Console.WriteLine("START");
            Console.WriteLine(start);
            Console.WriteLine();
            var open = new PriorityQueue<State, int>();
            var optimal_costs = new Dictionary<State, int>();
            open.Enqueue(start, start.costSoFar + H(start));
            optimal_costs[start] = 0;
            int nodes_visited = 0;


                while (open.Count > 0)
                {
                    var state = open.Dequeue();
                    if (state.IsEnd())
                    {
                        Console.WriteLine("Search Cost: {0}", nodes_visited);
                        return state;
                    }

                    state.GetNextPossibleStates().ForEach(state =>
                        {
                            int current_best = optimal_costs.GetValueOrDefault(state, int.MaxValue);

                            if(state.costSoFar < current_best)
                            {
                                optimal_costs[state] = state.costSoFar;
                                open.Enqueue(state, state.costSoFar + H(state));
                            }
                        }
                    );
                    nodes_visited++;
                }

            return start;
        }


        public static readonly Dictionary<(char room, char pod), int> COST_ESTIMATE;
        public static readonly Dictionary<char, int> STEP_COST;
        public static readonly Dictionary<char, int> ROOM_INDEX;

        static Day23()
        {
            ROOM_INDEX = new Dictionary<char, int>();
            ROOM_INDEX['A'] = 2;
            ROOM_INDEX['B'] = 4;
            ROOM_INDEX['C'] = 6;
            ROOM_INDEX['D'] = 8;


            STEP_COST = new Dictionary<char, int>();
            STEP_COST['A'] = 1;
            STEP_COST['B'] = 10;
            STEP_COST['C'] = 100;
            STEP_COST['D'] = 1000;

            COST_ESTIMATE = new Dictionary<(char room, char pod), int>();
            COST_ESTIMATE[('A', 'B')] = 40;
            COST_ESTIMATE[('A', 'C')] = 600;
            COST_ESTIMATE[('A', 'D')] = 8000;

            COST_ESTIMATE[('B', 'A')] = 4;
            COST_ESTIMATE[('B', 'C')] = 400;
            COST_ESTIMATE[('B', 'D')] = 6000;

            COST_ESTIMATE[('C', 'A')] = 6;
            COST_ESTIMATE[('C', 'B')] = 40;
            COST_ESTIMATE[('C', 'D')] = 4000;

            COST_ESTIMATE[('D', 'A')] = 8;
            COST_ESTIMATE[('D', 'B')] = 60;
            COST_ESTIMATE[('D', 'C')] = 400;
        }

        private int H(State state)
        {
            int H = 0;
            //return H;
            // Cost of moving from rooms
            H += state.rooms.Keys.Select(room => state.rooms[room].Where(pod => pod != room).Select(pod => COST_ESTIMATE[(room, pod)]).Sum()).Sum();
            // Cost of navigating hallway
            for (int i = 0; i < state.hallway.Length; i++)
            {
                if (state.hallway[i] != '.')
                {
                    H += Math.Abs(i - ROOM_INDEX[state.hallway[i]]) * STEP_COST[state.hallway[i]];
                }
            }
            return H;
        }

        public object SolvePart1()
        {
            var win_state =  SolveShortestPathSearch(Start);
            win_state.PrintPath();
            return win_state.costSoFar;
        }

        public object SolvePart2()
        {
            /* 
                #D#C#B#A#
                #D#B#A#C#
             */
            Start.roomCapacity = 4;
            char tmp;
            tmp = Start.roomA.Pop();
            Start.roomA.Push('D');
            Start.roomA.Push('D');
            Start.roomA.Push(tmp);

            tmp = Start.roomB.Pop();
            Start.roomB.Push('B');
            Start.roomB.Push('C');
            Start.roomB.Push(tmp);

            tmp = Start.roomC.Pop();
            Start.roomC.Push('A');
            Start.roomC.Push('B');
            Start.roomC.Push(tmp);

            tmp = Start.roomD.Pop();
            Start.roomD.Push('C');
            Start.roomD.Push('A');
            Start.roomD.Push(tmp);


            var win_state = SolveShortestPathSearch(Start);
            win_state.PrintPath();
            return win_state.costSoFar;
        }
    }
}
