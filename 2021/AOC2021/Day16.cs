using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    abstract class Expression
    {
        public readonly int Version;
        public readonly int TypeID;
        public readonly int BinaryLength;

        protected Expression(int version, int type, int binary_length)
        {
            Version = version;
            TypeID = type;
            BinaryLength = binary_length;
        }

        public abstract long Evaluate();

        public abstract int VersionNumberSum();


        public static Expression Parse(string binary)
        {
            Expression expression = null;
            var version = Convert.ToInt32(binary.Substring(0, 3), 2);
            var type = Convert.ToInt32(binary.Substring(3, 3), 2);
            switch (type)
            {
                case 4: // Literal value
                    var byte_idx = 1;
                    var number_bits = "";
                    do
                    {
                        byte_idx += 5;
                        number_bits += binary.Substring(byte_idx + 1, 4);
                    } while (binary[byte_idx] != '0');
                    expression = new Literal(version, type, byte_idx + 5, Convert.ToInt64(number_bits, 2));
                    break;
                default: // Operator
                    var lengt_type_id = binary[6] == '1' ? 1 : 0;
                    var header_end = lengt_type_id == 1 ? 18 : 22;
                    var operands_length = lengt_type_id == 1 ? Convert.ToInt32(binary.Substring(7, 11), 2) : Convert.ToInt32(binary.Substring(7, 15), 2);
                    var operands = new List<Expression>();
                    binary = binary.Substring(header_end);
                    while (operands_length > 0) 
                    {
                        var operand = Expression.Parse(binary);
                        operands.Add(operand);
                        binary = binary.Substring(operand.BinaryLength);
                        if (lengt_type_id == 1)
                        {
                            operands_length--;
                        }
                        else
                        {
                            operands_length -= operand.BinaryLength;
                        }
                    }

                    expression = new Operator(version, type, header_end + operands.Select(op => op.BinaryLength).Sum(), lengt_type_id, operands);
                    break;
            }
            return expression;
        }
    }

    class Operator : Expression
    {
        public Operator(int version, int type, int binary_length, int lengt_type_id, List<Expression> operands) : base(version, type, binary_length)
        {
            Operands = operands;
            LengthTypeID = lengt_type_id;
        }

        public readonly int LengthTypeID;
        public readonly List<Expression> Operands;

        public override long Evaluate()
        {
            switch (TypeID)
            {
                case 0:
                    return Operands.Select(op => op.Evaluate()).Sum();
                case 1:
                    return Operands.Select(op => op.Evaluate()).Aggregate(1l, (a, b) => a * b);
                case 2:
                    return Operands.Select(op => op.Evaluate()).Min();
                case 3:
                    return Operands.Select(op => op.Evaluate()).Max();
                case 5:
                    return Operands[0].Evaluate() > Operands[1].Evaluate() ? 1l : 0l;
                case 6:
                    return Operands[0].Evaluate() < Operands[1].Evaluate() ? 1l : 0l;
                case 7:
                    return Operands[0].Evaluate() == Operands[1].Evaluate() ? 1l : 0l;
                default:
                    throw new NotImplementedException();
            }
        }

        public override int VersionNumberSum()
        {
            return Version + Operands.Select(x => x.VersionNumberSum()).Sum();
        }
    }

    class Literal : Expression
    {
        public Literal(int version, int type, int length, long value) : base(version, type, length)
        {
            Value = value;
        }

        private readonly long Value;


        public override long Evaluate()
        {
            return Value;
        }

        public override int VersionNumberSum()
        {
            return Version;
        }
    }

    internal class Day16 : ISolvable
    {
        public Day16(string[] lines)
        {
            var packet = lines[0];
            var binary_str = string.Join(string.Empty, packet.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
            Expression = Expression.Parse(binary_str);
        }

        public Expression Expression { get; }

        public object SolvePart1()
        {
            return Expression.VersionNumberSum();
        }

        public object SolvePart2()
        {
            return Expression.Evaluate();
        }
    }
}
