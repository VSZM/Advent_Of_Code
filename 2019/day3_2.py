import numpy

def generatePointsBetween(point1, point2):
    points = []
    if point1[0] == point2[0]:# vertical line
        sign = numpy.sign(point2[1] - point1[1])
        for i in range(point1[1], point2[1] + sign, sign):
            points.append((point1[0], i))
    else:# horizontal line
        sign = numpy.sign(point2[0] - point1[0])
        for i in range(point1[0], point2[0] + sign, sign):
            points.append((i, point1[1]))

    return points

def dstSumToIntersection(points_1, points_2, intersection):
    dst1 = points_1.index(intersection)
    dst2 = points_2.index(intersection)

    return dst1 + dst2

def closestIntersection(instructions1, instructions2):
    points_1_l = [(0,0)]
    points_2_l = [(0,0)]
    points_1_s = set()
    points_2_s = set()

    cur_pos = (0,0)
    for instruction in instructions1:
        if instruction[0] == 'L':
            next_pos = (cur_pos[0] - int(instruction[1:]), cur_pos[1])
        elif instruction[0] == 'R':
            next_pos = (cur_pos[0] + int(instruction[1:]), cur_pos[1])
        elif instruction[0] == 'D':
            next_pos = (cur_pos[0], cur_pos[1] - int(instruction[1:]))
        elif instruction[0] == 'U':
            next_pos = (cur_pos[0], cur_pos[1] + int(instruction[1:]))

        vector_points = generatePointsBetween(cur_pos, next_pos)
        points_1_s.update(set(vector_points))
        points_1_l.extend(vector_points[1:])
        cur_pos = next_pos 

    cur_pos = (0,0)
    for instruction in instructions2:
        if instruction[0] == 'L':
            next_pos = (cur_pos[0] - int(instruction[1:]), cur_pos[1])
        elif instruction[0] == 'R':
            next_pos = (cur_pos[0] + int(instruction[1:]), cur_pos[1])
        elif instruction[0] == 'D':
            next_pos = (cur_pos[0], cur_pos[1] - int(instruction[1:]))
        elif instruction[0] == 'U':
            next_pos = (cur_pos[0], cur_pos[1] + int(instruction[1:]))
        
        vector_points = generatePointsBetween(cur_pos, next_pos)
        points_2_s.update(set(vector_points))
        points_2_l.extend(vector_points[1:])
        cur_pos = next_pos

    intersections = points_1_s.intersection(points_2_s)
    intersections.remove((0, 0))
    return min([dstSumToIntersection(points_1_l, points_2_l, intersection) for intersection in intersections])


if __name__ == "__main__":
    with open('day3.txt', 'r') as f:
        l1, l2 = f.readlines()
        l1 = l1.split(',')
        l2 = l2.split(',')
        print(closestIntersection(l1, l2))
