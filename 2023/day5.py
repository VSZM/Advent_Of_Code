file_path = 'input.txt'

with open(file_path, 'r') as file:
    content = file.readlines()

def parse_ranges(line):
    """ Parses a line containing ranges into a list of tuples (start, length). """
    parts = line.strip().split()
    return [(int(parts[i]), int(parts[i + 1])) for i in range(1, len(parts), 2)]

def parse_mapping(lines):
    """ Parses lines containing mapping data into a list of tuples (dest_start, src_start, length). """
    return [tuple(map(int, line.strip().split())) for line in lines]

def apply_mapping_to_range(src_range, mapping):
    """ Applies mapping to a single source range and returns a list of resulting ranges. """
    src_start, src_length = src_range
    resulting_ranges = []

    for dest_start, map_start, map_length in mapping:
        if src_start + src_length <= map_start or src_start >= map_start + map_length:
            # No overlap
            continue

        # Calculate the overlap
        overlap_start = max(src_start, map_start)
        overlap_end = min(src_start + src_length, map_start + map_length)
        overlap_length = overlap_end - overlap_start

        # Calculate the corresponding destination range
        dest_range_start = dest_start + (overlap_start - map_start)
        resulting_ranges.append((dest_range_start, overlap_length))

    if not resulting_ranges:
        # If no mapping was applied, the source range maps to itself
        resulting_ranges.append((src_start, src_length))

    return resulting_ranges

def process_ranges_through_maps(seed_ranges, maps):
    """ Processes seed ranges through all maps and returns the resulting location ranges. """
    current_ranges = seed_ranges

    for map_type in maps:
        new_ranges = []
        for src_range in current_ranges:
            new_ranges.extend(apply_mapping_to_range(src_range, maps[map_type]))
        current_ranges = new_ranges

    return current_ranges

def find_lowest_location(seed_ranges, maps):
    """ Finds the lowest location number from the processed seed ranges. """
    location_ranges = process_ranges_through_maps(seed_ranges, maps)
    return min(start for start, _ in location_ranges)

# Parsing the input file
seeds = parse_ranges(content[0])
maps = {}
current_map = None

for line in content[2:]:
    if 'map:' in line:
        current_map = line.strip().split(':')[0]
        maps[current_map] = []
    elif line.strip():
        maps[current_map].append(parse_mapping([line])[0])

# Finding the lowest location number
lowest_location = find_lowest_location(seeds, maps)
lowest_location



def process_input(content):
    """ Processes the input content into seeds and mapping tables. """
    seeds = parse_ranges(content[0])
    maps = {}
    current_map = None

    for line in content[2:]:
        if 'map:' in line:
            current_map = line.strip().split(':')[0]
            maps[current_map] = []
        elif line.strip():
            maps[current_map].append(parse_mapping([line])[0])

    return seeds, maps

def solve_challenge(content):
    """ Solves both parts of the challenge and returns their results. """
    seeds, maps = process_input(content)

    # Part One - Processing single seed numbers
    single_seeds = [(seed, 1) for seed, _ in seeds]
    lowest_location_part_one = find_lowest_location(single_seeds, maps)

    # Part Two - Processing ranges of seed numbers
    lowest_location_part_two = find_lowest_location(seeds, maps)

    return lowest_location_part_one, lowest_location_part_two

# Solving the challenge
result_part_one, result_part_two = solve_challenge(content)

print(f"Part 1> {result_part_one}\nPart 2> {result_part_two}")

