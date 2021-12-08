from intcode import interpretProgram, ThreadState, ProgramPause, readProgram
import numpy as np


def runProgram(memory):
    state = ThreadState(memory, 0, [], [])
    interpretProgram(state)

    print(state.output)
    return len(list(filter(lambda x: x == 2, state.output[2::3])))

def getPaddleX(state):
    game_state = [state.output[i:i + 3] for i in range(0, len(state.output), 3)]
    paddle = list(filter(lambda tile: tile[2] == 3, game_state))[0]

    return paddle[0]

def getBallX(state):
    game_state = [state.output[i:i + 3] for i in range(0, len(state.output), 3)]
    ball = list(filter(lambda tile: tile[2] == 4, game_state))[0]

    return ball[0]

def getScore(state):
    game_state = [state.output[i:i + 3] for i in range(0, len(state.output), 3)]
    score = list(filter(lambda tile: tile[0] == -1 and tile[1] == 0, game_state))[0]

    return score[2]
    

def runGame(memory):
    memory[0] = 2

    state = ThreadState(memory, 0, [0], [])
    
    while True:
        try:
            state.output = []
            interpretProgram(state)
            score = getScore(state)
            break
        except ProgramPause:
            ballx = getBallX(state)
            paddlex = getPaddleX(state)
            joystick = np.sign(ballx - paddlex)
            state.input_stream = [joystick]

    return score

if __name__ == "__main__":
    with open('day13.txt', 'r') as f:
        memory = readProgram(f)

    #part 1
    print(runProgram(memory.copy()))
    
    #part 2
    print(runGame(memory.copy()))