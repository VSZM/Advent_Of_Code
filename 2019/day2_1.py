from typing import List
import sys




def runProgram(instructions: List[int]):
    i = 0
    while instructions[i] != 99:
        pos = instructions[i + 3]
        a = instructions[instructions[i + 1]]
        b = instructions[instructions[i + 2]]

        if instructions[i] == 1:
            instructions[pos] = a + b
        elif instructions[i] == 2:
            instructions[pos] = a * b
        else:
            raise ValueError(f'Unexpected operation {instructions[i]} at {i}')
    
        i += 4
    
    print(instructions)
    return instructions[0]


if __name__ == "__main__":
    instructions = [int(num) for num in sys.stdin.readline().split(',')]
    instructions[1] = 12
    instructions[2] = 2
    print(runProgram(instructions))