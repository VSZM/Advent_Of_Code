using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_Of_Code_11_20
{
    internal enum InstructionEnum
    {
        Hlf,
        Tpl,
        Inc,
        Jmp,
        Jie,
        Jio
    }

    internal class Instruction
    {
        public InstructionEnum InstructionType;
        public char Register;
        public int Value;

        public Instruction(string line)
        {
            var splitted_line = line.Split();
            switch (splitted_line[0])
            {
                case "hlf":
                    InstructionType = InstructionEnum.Hlf;
                    Register = splitted_line[1][0];
                    break;
                case "tpl":
                    InstructionType = InstructionEnum.Tpl;
                    Register = splitted_line[1][0];
                    break;
                case "inc":
                    InstructionType = InstructionEnum.Inc;
                    Register = splitted_line[1][0];
                    break;
                case "jmp":
                    InstructionType = InstructionEnum.Jmp;
                    Value = int.Parse(splitted_line[1]);
                    break;
                case "jie":
                    InstructionType = InstructionEnum.Jie;
                    Register = splitted_line[1][0];
                    Value = int.Parse(splitted_line[2]);
                    break;
                case "jio":
                    InstructionType = InstructionEnum.Jio;
                    Register = splitted_line[1][0];
                    Value = int.Parse(splitted_line[2]);
                    break;
                default:
                    throw new IHaveABadFeelingAboutThisException("Unexpected line: " + line);
            }
        }
    }

    internal class Day23Interpreter : ISolvable
    {
        private readonly Dictionary<char, UInt64> _registers = new Dictionary<char, UInt64>();

        public string Solve(string[] inputLines, bool isPart2)
        {
            List<Instruction> instructions = inputLines.Select(line => new Instruction(line)).ToList();
            if (isPart2)
                _registers['a'] = 1;
            else
                _registers['a'] = 0;
            _registers['b'] = 0;

            int i = 0;
            while (i < instructions.Count)
            {
                var instruction = instructions[i];
                switch (instruction.InstructionType)
                {
                    case InstructionEnum.Hlf:
                        _registers[instruction.Register] /= 2;
                        break;
                    case InstructionEnum.Tpl:
                        _registers[instruction.Register] *= 3;
                        break;
                    case InstructionEnum.Inc:
                        _registers[instruction.Register]++;
                        break;
                    case InstructionEnum.Jmp:
                        i += instruction.Value;
                        continue;
                    case InstructionEnum.Jie:
                        i += _registers[instruction.Register] % 2 == 0 ? instruction.Value : 1;
                        continue;
                    case InstructionEnum.Jio:
                        i += _registers[instruction.Register] == 1 ? instruction.Value : 1;
                        continue;
                }
                ++i;
            }

            return _registers['b'].ToString();
        }
    }
}
