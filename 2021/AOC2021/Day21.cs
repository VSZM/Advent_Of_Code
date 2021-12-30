using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AOC2021
{

    class Node<T>
    {
        public Node(T v, Node<T> next)
        {
            this.Value = v;
        }

        public T Value { get; set; }
        public Node<T> Next { get; set; }

        public static Node<T> operator ++(Node<T> node)
        {
            return node.Next;
        }

    }

    internal class Day21 : ISolvable
    {

        public Day21(string[] lines)
        {
            Player1StartPosition = int.Parse(lines[0].Split().Last());
            Player2StartPosition = int.Parse(lines[1].Split().Last());
            Memo = new Dictionary<(int player1_score_remaining, int player2_score_remaining, int player1_pos, int player2_pos), (long player1_wins, long player2_wins)>();
        }

        public int Player1StartPosition { get; }
        public int Player2StartPosition { get; }
        public Dictionary<(int player1_score_remaining, int player2_score_remaining, int player1_pos, int player2_pos), (long player1_wins, long player2_wins)> Memo { get; }

        public object SolvePart1()
        {
            var player1_pos = Player1StartPosition;
            var player2_pos = Player2StartPosition;
            var player1_score = 0;
            var player2_score = 0;
            var roll_count = 0;
            var is_player1_turn = true;
            var die = new Node<int>(1, null);
            var previous = die;
            Enumerable.Range(2, 99).ToList().ForEach(x =>
            {
                var next = new Node<int>(x, null);
                previous.Next = next;
                previous = next;
                if (x == 100)
                    next.Next = die;
            });

            while (player1_score < 1000 && player2_score < 1000)
            {
                if (is_player1_turn)
                {
                    player1_pos = Step(player1_pos, die++.Value + die++.Value + die++.Value);
                    player1_score += player1_pos;
                    roll_count += 3;
                }
                else
                {
                    player2_pos = Step(player2_pos, die++.Value + die++.Value + die++.Value);
                    player2_score += player2_pos;
                    roll_count += 3;
                }
                is_player1_turn = !is_player1_turn;
            }
            return Math.Min(player1_score, player2_score) * roll_count;
        }

        private static readonly IReadOnlyList<int> DIRAC_POSSIBILITIES = Enumerable.Range(1, 3).SelectMany(x1 => Enumerable.Range(1, 3).SelectMany(x2 => Enumerable.Range(1, 3).Select(x3 => x1 + x2 + x3))).ToList();
        private static readonly object MEMO_LOCK = new object();

        private int Step(int pos, int roll)
        {
            pos = (pos + roll) % 10;
            if (pos == 0)
                pos = 10;
            return pos;
        }

        private (long player1_wins, long player2_wins) Solve_Multiverse(int player1_score_remaining, int player2_score_remaining, int player1_pos, int player2_pos)
        {
            if (player1_score_remaining <= 0)
            {
                return (1, 0);
            }
            if (player2_score_remaining <= 0)
            {
                return (0, 1);
            }
            var key = (player1_score_remaining, player2_score_remaining, player1_pos, player2_pos);
            if (Memo.ContainsKey(key))
                return Memo[key];


            var mutex = new object();
            var player1_wins = 0L;
            var player2_wins = 0L;
            Parallel.ForEach(DIRAC_POSSIBILITIES, roll =>
            {
                var new_pos = Step(player1_pos, roll);
                var new_score = player1_score_remaining - new_pos;
                var outcome = Solve_Multiverse(player2_score_remaining, new_score, player2_pos, new_pos);
                Monitor.Enter(mutex);
                player1_wins += outcome.player2_wins;
                player2_wins += outcome.player1_wins;
                Monitor.Exit(mutex);
            });



            Monitor.Enter(MEMO_LOCK);
            Memo[key] = (player1_wins, player2_wins);
            Monitor.Exit(MEMO_LOCK);
            return (player1_wins, player2_wins);
        }

        public object SolvePart2()
        {
            var wincounts = Solve_Multiverse(21, 21, Player1StartPosition, Player2StartPosition);
            return Math.Max(wincounts.player1_wins, wincounts.player2_wins);
        }
    }
}
