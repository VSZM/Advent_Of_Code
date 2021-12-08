from intcode import readProgram, interpretProgram, ThreadState, ProgramPause
import itertools

def calculateSignal(memory, permutation):
    previous_output = 0
    
    threads = [ThreadState(memory.copy(), 0, [phase], [], name) for phase, name in zip(permutation, ['A', 'B', 'C', 'D', 'E'])]
    feedback_loop = itertools.cycle(threads)

    for state in feedback_loop:
        try:
            state.input_stream.append(previous_output)
            #print(f'Starting {state.name} with input {state.input_stream}. IP = {state.ip}')
            interpretProgram(state)
            if (state.name == 'E'):
                break
        except ProgramPause:
            pass
            #print(f'Pausing {state.name}. Output = {state.output[-1]}. IP = {state.ip}')
        finally:
            previous_output = state.output[-1]
        
    return threads[-1].output[-1]

def findConfiguration(memory):
    all_permutations = list(itertools.permutations(range(5,10), 5))
    max_signal = 0

    for permutation in all_permutations:
        signal = calculateSignal(memory, permutation)
        if signal > max_signal:
            max_signal = signal
            print(f'New max signal of strength |{signal}| for config |{permutation}|')

    print(f'Max signal: |{max_signal}|')

if __name__ == "__main__":
    with open('day7.txt', 'r') as f:
        memory = readProgram(f)
    
    findConfiguration(memory)