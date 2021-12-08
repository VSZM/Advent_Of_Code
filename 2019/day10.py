import math
from itertools import cycle

def visibilityScore(points, current_point):
    return len(set([math.atan2(current_point[1] - point[1], current_point[0] - point[0]) for point in points]))



def calculateBestStation(asteroids):
    max = 0
    max_point = None

    for point in asteroids:
        current_visibility = visibilityScore(asteroids - set([point]), point)
        if current_visibility > max:
            max = current_visibility
            max_point = point

    return max, max_point

def mapAsteroidsToTangents(asteroids, pov):
    tangent_map = dict()

    for asteroid in asteroids:
        tangent = math.atan2(pov[1] - asteroid[1], pov[0] - asteroid[0])
        if tangent in tangent_map:
            tangent_map[tangent].append(asteroid)
        else:
            tangent_map[tangent] = [asteroid]

    # sorting tangent lines based on distance from pov
    for tangent in tangent_map.keys():
        tangent_map[tangent] = sorted(tangent_map[tangent],\
            key = lambda asteroid: math.sqrt((pov[0] - asteroid[0]) ** 2 + (pov[1] - asteroid[1]) ** 2))

    rotate90 = lambda radian: radian - math.radians(90)
    normalize = lambda radian: radian if 0 <= radian <= math.radians(360) else radian + math.radians(360)
    return {normalize(rotate90(key)):tangent_map[key] for key in tangent_map.keys()}


def vaporizeAsteroids(asteroids, station, remaining_shots = -1):
    last_vaporized = None
    tangent_map = mapAsteroidsToTangents(asteroids, station)
    asteroid_angles = cycle(sorted(tangent_map.keys()))
    cannon_angle = next(asteroid_angles)

    while remaining_shots != 0 and len(tangent_map) != 0:
        if cannon_angle in tangent_map:
            last_vaporized = tangent_map[cannon_angle][0]
            tangent_map[cannon_angle] = tangent_map[cannon_angle][1:]
            if len(tangent_map[cannon_angle]) == 0:
                del tangent_map[cannon_angle]
            remaining_shots -= 1

        cannon_angle = next(asteroid_angles)

    return last_vaporized

if __name__ == "__main__":
    with open('day10.txt', 'r') as f:
        lines = f.readlines()
        points = []
        for y in range(len(lines)):
            for x in range(len(lines[y])):
                if lines[y][x] == '#':
                    points.append((x, y))
    
    max_score, max_point = calculateBestStation(set(points))
    print(max_score)
    vaporized_200th = vaporizeAsteroids(set(points) - set(max_point), max_point, 200)
    print(100 * vaporized_200th[0] + vaporized_200th[1])