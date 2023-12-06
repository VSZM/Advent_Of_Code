# Let's start by reading the contents of the provided input file
file_path = 'input.txt'

# Function to extract the first and last digit from a string and convert it to a two-digit number
def extract_calibration_value(line):
    # Extracting the first digit
    first_digit = next((char for char in line if char.isdigit()), None)
    # Extracting the last digit
    last_digit = next((char for char in reversed(line) if char.isdigit()), None)

    # Combining the digits to form a two-digit number, if both digits are found
    if first_digit and last_digit:
        return int(first_digit + last_digit)
    else:
        return 0

# Reading the file and calculating the sum of calibration values
total_sum = 0
with open(file_path, 'r') as file:
    for line in file:
        total_sum += extract_calibration_value(line.strip())

print(total_sum)

################################################
# PART2
################################################

# Mapping of spelled-out digits to their numeric equivalents
digit_mapping = {
    'one': '1', 'two': '2', 'three': '3', 'four': '4', 'five': '5',
    'six': '6', 'seven': '7', 'eight': '8', 'nine': '9'
}

# Function to replace spelled-out digits with their numeric equivalents
def convert_spelled_digits(line):
    ret = ""
    for i in range(len(line)):
        for k, v in digit_mapping.items():
            if line[i:].startswith(k):
                ret += v
                break
        else:
            ret += line[i]
    return ret

# Function to extract calibration value considering spelled-out digits
def extract_calibration_value_v2(line):
    # Replacing spelled-out digits with numeric equivalents
    line_fixed = convert_spelled_digits(line)
    #print(f"{line} -> {line_fixed}")

    # Extracting the first and last digit
    first_digit = next((char for char in line_fixed if char.isdigit()), None)
    last_digit = next((char for char in reversed(line_fixed) if char.isdigit()), None)

    # Combining the digits to form a two-digit number, if both digits are found
    if first_digit and last_digit:
        return int(first_digit + last_digit)
    else:
        raise f"Error for line: {line}. Replaced to: {line_fixed}"

# Reading the file and calculating the sum of calibration values with the updated rules
total_sum_v2 = 0
with open(file_path, 'r') as file:
    for line in file:
        total_sum_v2 += extract_calibration_value_v2(line.strip())

print(total_sum_v2)
