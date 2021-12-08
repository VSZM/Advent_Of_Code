import sys
import math

def calculateFuel(n):
    if n <= 0:
        return 0

    fuel = n // 3 - 2
    if fuel <= 0:
        fuel = 0
    return fuel + calculateFuel(fuel)

print(sum([calculateFuel(int(line)) for line in sys.stdin.readlines()]))
