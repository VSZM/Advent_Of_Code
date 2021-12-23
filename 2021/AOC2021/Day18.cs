using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    class TreeNode
    {
        public int? value;
        public TreeNode parent;
        public TreeNode left;
        public TreeNode right;

        public int Depth
        {
            get
            {
                if (parent == null)
                    return 1;
                return parent.Depth + 1;
            }
        }

        public bool Splittable { get { return value != null && value >= 10; } }
        public bool Explodable { get { return Depth > 4 && !isLeaf; } }
        public bool isLeaf { get { return value != null; } }


        public TreeNode(TreeNode parent, TreeNode left, TreeNode right)
        {
            this.parent = parent;
            this.left = left;
            this.right = right;
        }

        public TreeNode(TreeNode parent, int value)
        {
            this.parent = parent;
            this.value = value;
        }

        public void Explode()
        {
            // When exploding leftwards we look for the rightmost node that is left of the current one.
            // We prioritize right nodes, then left nodes, then parent nodes for expanding our search.
            // We can only go right from the root node, so we exclude the right side of everything from current node to the root
            // Vice versa all for other side
            var q = new PriorityQueue<TreeNode, int>();
            var path_to_root = new HashSet<TreeNode>();
            var visited = new HashSet<TreeNode>();
            q.Enqueue(this.parent, 3);
            visited.Add(this);
            var current = this;
            while (current != null)
            {
                path_to_root.Add(current);
                current = current.parent;
            }

            while (q.Count > 0)
            {
                TreeNode node = q.Dequeue();
                if (visited.Contains(node))
                    continue;

                visited.Add(node);

                if (node.isLeaf)
                {
                    node.value += left.value;
                    break;
                }

                if (node.right != null && !path_to_root.Contains(node) && !visited.Contains(node.right))
                    q.Enqueue(node.right, 1);
                if (node.left != null && !visited.Contains(node.left))
                    q.Enqueue(node.left, 2);
                if (node.parent != null && !visited.Contains(node.parent))
                    q.Enqueue(node.parent, 3);
            }


            // Exploding rightwards

            q = new PriorityQueue<TreeNode, int>();
            visited = new HashSet<TreeNode>();
            q.Enqueue(this.parent, 3);
            visited.Add(this);


            while (q.Count > 0)
            {
                TreeNode node = q.Dequeue();
                if (visited.Contains(node))
                    continue;

                visited.Add(node);

                if (node.isLeaf)
                {
                    node.value += right.value;
                    break;
                }

                if (node.left != null && !path_to_root.Contains(node) && !visited.Contains(node.left))
                    q.Enqueue(node.left, 1);
                if (node.right != null && !visited.Contains(node.right))
                    q.Enqueue(node.right, 2);
                if (node.parent != null && !visited.Contains(node.parent))
                    q.Enqueue(node.parent, 3);
            }

            this.value = 0;
            this.left = null;
            this.right = null;
        }

        public void Split()
        {
            if (value == null || left != null || right != null)
                throw new AccessViolationException("This node is not splittable!");

            left = new TreeNode(this, value.Value / 2);
            right = new TreeNode(this, (int)Math.Ceiling(value.Value / 2.0));
            value = null;
        }

        public static TreeNode Add(TreeNode l, TreeNode r)
        {
            // Making copies, so Add doesnt mutate original left and right
            var left = TreeNode.FromString(null, l.ToString());
            var right = TreeNode.FromString(null, r.ToString());

            var ret = new TreeNode(null, left, right);
            left.parent = ret;
            right.parent = ret;
            //Console.WriteLine("after addition:\t{0}", ret);
            ret.Reduce();
            return ret;
        }

        private void Reduce()
        {
            var operation_happened = false;
            do
            {
                operation_happened = false;
                TreeNode explode_node = FindExplodeNode();
                TreeNode split_node = FindSplitNode();
                if (explode_node != null)
                {
                    explode_node.Explode();
                    //Console.WriteLine("after explode:\t{0}", this);
                    operation_happened = true;
                    continue;
                }
                if (split_node != null)
                {
                    split_node.Split();
                    //Console.WriteLine("after split:\t{0}", this);
                    operation_happened = true;
                    continue;
                }

            } while (operation_happened);
        }

        private TreeNode FindSplitNode()
        {
            if (left != null)
            {
                var left_splittable = left.FindSplitNode();
                if (left_splittable != null)
                    return left_splittable;
            }
            if (Splittable)
            {
                return this;
            }
            if (right != null)
            {
                var right_splittable = right.FindSplitNode();
                if (right_splittable != null)
                    return right_splittable;
            }
            return null;
        }

        private TreeNode FindExplodeNode()
        {
            if (left != null)
            {
                var left_explodable = left.FindExplodeNode();
                if (left_explodable != null)
                    return left_explodable;
            }
            if (Explodable)
            {
                return this;
            }

            if (right != null)
            {
                var right_explodable = right.FindExplodeNode();
                if (right_explodable != null)
                    return right_explodable;
            }
            return null;
        }

        public static TreeNode FromString(TreeNode parent, string str)
        {
            str = str.Replace(" ", "");
            var root = new TreeNode(parent, null, null);
            if (str.Contains(','))
            {
                str = str.Substring(1, str.Length - 2);
                var mid_point = FindMidPoint(str);
                root.left = TreeNode.FromString(root, str.Substring(0, mid_point));
                root.right = TreeNode.FromString(root, str.Substring(mid_point + 1));
            }
            else
            {
                root.value = int.Parse(str);
            }
            return root;
        }

        private static int FindMidPoint(string str)
        {
            int lowest_idx = -1;
            int lowest_depth = int.MaxValue;
            int current_depth = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '[')
                    current_depth++;
                else if (str[i] == ']')
                    current_depth--;
                else if (str[i] == ',')
                    if (current_depth < lowest_depth)
                    {
                        lowest_depth = current_depth;
                        lowest_idx = i;
                    }
            }
            return lowest_idx;
        }

        public override string ToString()
        {
            if (value == null)
            {
                return string.Format("[{0}, {1}]", left.ToString(), right.ToString());
            }
            return value.ToString();
        }

        public int Magnitude()
        {
            if (isLeaf)
                return value.Value;

            return 3 * left.Magnitude() + 2 * right.Magnitude();
        }
    }

    internal class Day18 : ISolvable
    {
        public Day18(string[] lines)
        {
            Expressions = lines.Select(line => TreeNode.FromString(null, line)).ToList();
            /*foreach (var exp in Expressions)
            {
                Console.WriteLine("{0} Magnitude: {1}", exp, exp.Magnitude());
            }*/
        }

        public List<TreeNode> Expressions { get; }

        public object SolvePart1()
        {
            var added = Expressions.Aggregate((a, b) => TreeNode.Add(a, b));
            Console.WriteLine(added);
            return added.Magnitude();
        }

        public object SolvePart2()
        {
            int max = 0;
            for (int i = 0; i < Expressions.Count; i++)
            {
                for (int j = 0; j < Expressions.Count; j++)
                {
                    if (i == j)
                        continue;
                    var forward = TreeNode.Add(Expressions[i], Expressions[j]).Magnitude();
                    var backward = TreeNode.Add(Expressions[j], Expressions[i]).Magnitude();
                    if(forward > max)
                        max = forward;
                    if(backward > max)
                        max = backward;
                }
            }
            return max;
        }
    }
}
