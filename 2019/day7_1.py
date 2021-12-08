from intcode import readProgram, interpretProgram, ThreadState
import itertools
from tqdm import tqdm

def calculateSignal(memory, permutation):
    output = interpretProgram(ThreadState(memory.copy(), 0, [permutation[0], 0], []))[0]
    for phase in permutation[1:]:
        output = interpretProgram(ThreadState(memory.copy(), 0, [phase, output], []))[0]

    return output

def findConfiguration(memory):
    all_permutations = list(itertools.permutations(range(0,5), 5))
    max_signal = 0

    for permutation in tqdm(all_permutations):
        signal = calculateSignal(memory, permutation)
        if signal > max_signal:
            max_signal = signal
            print(f'New max signal of strength |{signal}| for config |{permutation}|')

    print(f'Max signal: |{max_signal}|')

if __name__ == "__main__":
    with open('day7.txt', 'r') as f:
        memory = readProgram(f)
    
    findConfiguration(memory)