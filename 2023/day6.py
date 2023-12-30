def calculate_winning_ways(time, distance):
    """Calculate the number of ways to win the race."""
    ways_to_win = 0
    for hold_time in range(time):
        travel_time = time - hold_time
        if (hold_time * travel_time) > distance:
            ways_to_win += 1
    return ways_to_win

def main(input_file):
    with open(input_file, 'r') as file:
        lines = file.readlines()

    # Part One: Multiple short races
    time_values = map(int, lines[0].split()[1:])
    distance_values = map(int, lines[1].split()[1:])

    total_ways_part_one = 1
    for time, record_distance in zip(time_values, distance_values):
        ways = calculate_winning_ways(time, record_distance)
        total_ways_part_one *= ways

    # Part Two: Single long race
    race_time = int("".join(lines[0].split()[1:]))
    record_distance = int("".join(lines[1].split()[1:]))
    total_ways_part_two = calculate_winning_ways(race_time, record_distance)

    return total_ways_part_one, total_ways_part_two

# Example usage
result_part_one, result_part_two = main("input.txt")
print("Part One:", result_part_one)
print("Part Two:", result_part_two)

# Uncomment and run the above lines with the actual 'input.txt' file to get the results.
