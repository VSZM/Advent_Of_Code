from functools import reduce

def getLayers(numbers, width, height):
    layersize = width * height
    layers = []
    for start in range(0, len(numbers), layersize):
        layers.append(numbers[start:start + layersize])
    return layers

def analyze(numbers, width, height):
    layers = getLayers(numbers, width, height)
    
    countzeros = lambda li: len([number for number in li if number == 0])
    layers_with_zeroes = sorted([(countzeros(layer), layer) for layer in layers])

    selected = layers_with_zeroes[0][1]

    filter_num = lambda li, criteria: [number for number in li if number == criteria]
    return len(filter_num(selected, 1)) * len(filter_num(selected, 2))

def pixelAdd(left, right):
    if left in [0, 1]:
        return left
    return right

def render(numbers, width, height):
    layers = getLayers(numbers, width, height)
    layersize = len(layers[0])

    image = []
    for i in range(layersize):
        pixel = reduce(pixelAdd, [layer[i] for layer in layers])
        image.append(pixel)

    for m in range(height):
        print(''.join([str(num)*3 for num in image[m * width:m*width + width]]))
        print(''.join([str(num)*3 for num in image[m * width:m*width + width]]))
        print(''.join([str(num)*3 for num in image[m * width:m*width + width]]))
        




if __name__ == "__main__":
    with open('day8.txt', 'r') as f:
        numbers = [int(char) for char in str(f.readline())]
    print(analyze(numbers, 25, 6))
    render(numbers, 25, 6)
