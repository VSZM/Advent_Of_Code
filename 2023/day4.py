from tqdm import tqdm

def count_matches(winning_numbers, your_numbers):
    return len(winning_numbers.intersection(your_numbers))

def process_part_one(cards):
    total_points = 0
    for card in cards:
        winning_numbers = {int(num) for num in card[0].split() if num.isdigit()}
        your_numbers = {int(num) for num in card[1].split() if num.isdigit()}
        matches = count_matches(winning_numbers, your_numbers)
        total_points += 2 ** (matches - 1) if matches else 0
    return total_points

def process_part_two(cards):
    # Initialize a dictionary to hold the count of each card
    card_counts = {i: 1 for i in range(len(cards))}

    # Iterate over each card based on its count
    for i in range(len(cards)):
        card = cards[i]
        winning_numbers = {int(num) for num in card[0].split() if num.isdigit()}
        your_numbers = {int(num) for num in card[1].split() if num.isdigit()}
        matches = count_matches(winning_numbers, your_numbers)

        # Update the counts for subsequent cards
        for j in range(1, matches + 1):
            next_card_index = i + j
            if next_card_index < len(cards):
                card_counts[next_card_index] += card_counts[i]

    # Sum the counts of all cards, which includes originals and won copies
    return sum(card_counts.values())

def solve(file_path):
    with open(file_path, 'r') as file:
        cards = [line.strip().split('|') for line in file]

    part_one_result = process_part_one(cards.copy())
    part_two_result = process_part_two(cards.copy())

    return part_one_result, part_two_result

file_path = 'input.txt'
part_one_result, part_two_result = solve(file_path)
print(f"Total points in Part 1: {part_one_result}")
print(f"Total scratchcards in Part 2: {part_two_result}")
