from intcode import interpretProgram, ThreadState, ProgramPause, readProgram
import matplotlib.pyplot as plt


def rotateLeft(direction):
    return [-1 * direction[1], direction[0]]


def rotateRight(direction):
    return [direction[1], -1 * direction[0]]

def readPixel(painting, position):
    if position not in painting:
        return 0
    
    return painting[position]

def runProgram(memory, painting):
    direction = [0, 1]
    position = (0, 0)

    state = ThreadState(memory, 0, [], [])

    while True:
        try:
            pixel = readPixel(painting, position)
            state.input_stream.append(pixel)
            interpretProgram(state)
            break
        except ProgramPause:
            color, turn = state.output[-2:]
            painting[position] = color
            
            if turn == 0:
                direction = rotateLeft(direction)
            else:
                direction = rotateRight(direction)
            position = (position[0] + direction[0], position[1] + direction[1])

    color = state.output[-2]
    painting[position] = color


def renderPainting(painting):
    white_coords = [xy for xy in painting.keys() if painting[xy] == 1]
    x = [xy[0] for xy in white_coords]
    y = [xy[1] for xy in white_coords]

    plt.scatter(x, y)
    plt.show()


if __name__ == "__main__":
    with open('day11.txt', 'r') as f:
        memory = readProgram(f)

    #part 1
    painting = dict()
    runProgram(memory.copy(), painting)
    print(len(painting))

    # part 2
    painting = {(0, 0): 1}
    runProgram(memory.copy(), painting)
    renderPainting(painting)