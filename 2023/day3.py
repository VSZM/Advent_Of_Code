import copy

"""
467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..
"""

def is_symbol(char):
    return char != '.' and not char.isdigit()

def is_inside_matrix(matrix, row, col):
    return 0 <= row < len(matrix) and 0 <= col < len(matrix[row])

def extract_number_from_row(matrix, row, col, remove = True):
    # first go to the leftmost digit in the matrix
    while col > 0 and matrix[row][col - 1].isdigit():
        col -= 1
    # extract the decimal number going to the right
    number = 0
    while col < len(matrix[row]) and matrix[row][col].isdigit():
        number = number * 10 + int(matrix[row][col])
        col += 1
        # remove the digit from the matrix
        if remove:
            matrix[row][col-1] = '.'

    #print(number)
    return number

def sum_numbers_next_to_symbol(matrix):
    sum = 0
    for row in range(len(matrix)):
        for col in range(len(matrix[row])):
            if is_symbol(matrix[row][col]):
                for dx in [-1, 0, 1]:
                    for dy in [-1, 0, 1]:
                        i, j = row + dx, col + dy
                        if is_inside_matrix(matrix, i, j) and matrix[i][j].isdigit():
                            #print(matrix[row][col])
                            sum += extract_number_from_row(matrix, i, j)
    return sum


def extract_start_of_number(matrix, row, col):
    # go to the leftmost digit in the row
    while col > 0 and matrix[row][col - 1].isdigit():
        col -= 1
   
    return row, col

def search_stars_with_2_adjacent_numbers(matrix):
    sum = 0
    for row in range(len(matrix)):
        for col in range(len(matrix[row])):
            if matrix[row][col] == '*':
                neighbors = [] # list of number positions, where pos is the start pos of the number, making it unique in pos instead of numerically
                for dx in [-1, 0, 1]:
                    for dy in [-1, 0, 1]:
                        i, j = row + dx, col + dy
                        if is_inside_matrix(matrix, i, j) and matrix[i][j].isdigit():
                            neighbors.append(extract_start_of_number(matrix, i, j))
                neighbors = list(set(neighbors))
                if len(neighbors) == 2:
                    num1 = extract_number_from_row(matrix, neighbors[0][0], neighbors[0][1], False)
                    num2 = extract_number_from_row(matrix, neighbors[1][0], neighbors[1][1], False)
                    print(num1, num2)
                    sum += num1 * num2
    return sum

if __name__ == "__main__":
    with open('input.txt', 'r') as file:
        matrix_str = [list(line.strip()) for line in file.readlines()]            
    #print(matrix_str)
    print(sum_numbers_next_to_symbol(copy.deepcopy(matrix_str)))
    print(search_stars_with_2_adjacent_numbers(copy.deepcopy(matrix_str)))
