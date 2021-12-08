from typing import List
from intcode import interpretProgram, ThreadState, readProgram
import sys




def runProgram(memory: List[int]):
    state = ThreadState(memory, 0, [5], [])
    output = interpretProgram(state)
    print(output)


if __name__ == "__main__":
    with open('day5.txt', 'r') as f:
        memory = readProgram(f)
        runProgram(memory)