from typing import List
from intcode import interpretProgram, ThreadState
import sys


def runProgram(memory: List[int]):
    state = ThreadState(memory, 0, [2], [])
    output = interpretProgram(state)
    print(output)


def readProgram(file) -> List[int]:
    return [int(num) for num in file.readline().split(',')]

if __name__ == "__main__":
    with open('day9.txt', 'r') as f:
        memory = readProgram(f)
        runProgram(memory)