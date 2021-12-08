import sys
import math

print(sum([math.floor(float(line)/3) - 2 for line in sys.stdin.readlines()]))