from typing import List
import sys
from itertools import product



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
    
    return instructions[0]


if __name__ == "__main__":
    instructions = [int(num) for num in sys.stdin.readline().split(',')]
    
    for noun, verb in product(range(100), range(100)):
        trial = instructions.copy()
        trial[1] = noun
        trial[2] = verb
        first_num = runProgram(trial)
        if first_num == 19690720:
            print(f'Found solution! noun, verb = {noun, verb}')
            break

    print(f'Solution is {100 * noun + verb}')