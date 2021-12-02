#!/usr/bin/python
# encoding: utf-8


#Visualization of the part1 example on http://adventofcode.com/day/15
"""
Butterscotch: capacity -1, durability -2, flavor 6, texture 3, calories 8
Cinnamon: capacity 2, durability 3, flavor -2, texture -1, calories 3

(-1 -2  6  3 )-1   * (x y)-1 = tmp
( 2  3 -2 -1 )   

z = tmp[0] * tmp[1] * tmp[2] * tmp[3]
"""

from matplotlib import pyplot as plt    
from pylab import *
from mpl_toolkits.mplot3d import axes3d

x_vals = []
y_vals = []
z_vals = []
z_alter = []

def calculate_points():
	global x_vals, y_vals, z_vals
	for x in range(0,101):
		y = 100 - x
		tmp = [max(0,x * -1 + y * 2), max(0,x * -2 + y * 3), max(0,x * 6 + y * -2), max(0,x * 3 + y * -1)]
		tmp2 = [x * -1 + y * 2, x * -2 + y * 3, x * 6 + y * -2, x * 3 + y * -1]		
		z = tmp[0] * tmp[1] * tmp[2] * tmp[3]
		z_alt = tmp2[0] * tmp2[1] * tmp2[2] * tmp2[3] 
		x_vals.append(x)
		y_vals.append(y)
		z_vals.append(z)		
		z_alter.append(z_alt)

def print_max():
	global x_vals, y_vals, z_vals
	max_pos = 0
	max_value = 0

	for i in range(0,101):
		if z_vals[i] > max_value:
			max_value = z_vals[i]
			max_pos = i

	print "Best combination: x = {} y = {} which gives {}".format(x_vals[max_pos], y_vals[max_pos], z_vals[max_pos])
	

def visualize_values():
	fig = figure(figsize=(10, 10))
	ax = fig.gca()
	ax.get_xaxis().get_major_formatter().set_scientific(False)
	ax.get_yaxis().get_major_formatter().set_scientific(False)

	ax.plot(x_vals,z_vals,'.')  
     
	fig.suptitle('Day15 part1 example visualization', fontsize=20)
	plt.xlabel('Butterscotch (Cinnamon is 100 - Butterscotch)', fontsize=18)
	plt.ylabel('Score', fontsize=16)
                                      
	#ax.plot(x_vals,z_alter,'.')                                                         
	ax.figure.canvas.draw()
	savefig('visualized.png', dpi=100)
	matplotlib.pyplot.close(fig)
		

if __name__ == "__main__":
	calculate_points()
	print_max()
	visualize_values()
	
