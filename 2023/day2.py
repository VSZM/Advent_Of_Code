def analyze_game(game_id, game_data, max_red, max_green, max_blue):
    min_red, min_green, min_blue = 0, 0, 0
    possible = True

    sets = game_data.split(';')
    for set in sets:
        red, green, blue = 0, 0, 0
        colors = set.strip().split(',')
        for color in colors:
            count, color_name = color.strip().split()
            count = int(count)
            if color_name == 'red':
                red = count
            elif color_name == 'green':
                green = count
            elif color_name == 'blue':
                blue = count
            # Update minimum cubes required
            min_red = max(min_red, red)
            min_green = max(min_green, green)
            min_blue = max(min_blue, blue)
        
        # Check if the game is possible with given maximums
        if red > max_red or green > max_green or blue > max_blue:
            possible = False

    power = min_red * min_green * min_blue
    return possible, game_id if possible else 0, power

def solve_cube_conundrum(filename):
    max_red, max_green, max_blue = 12, 13, 14
    total_sum_ids = 0
    total_power_sum = 0

    with open(filename, 'r') as file:
        for line in file:
            parts = line.strip().split(':')
            game_id = int(parts[0].split()[1])
            game_data = parts[1]
            
            possible, id_sum, power = analyze_game(game_id, game_data, max_red, max_green, max_blue)
            total_sum_ids += id_sum
            total_power_sum += power

    return total_sum_ids, total_power_sum

# Call the function with the input file
sum_of_ids, total_power_sum = solve_cube_conundrum('input.txt')
print(f"Sum of IDs of possible games: {sum_of_ids}")
print(f"Sum of the power of minimum sets: {total_power_sum}")
