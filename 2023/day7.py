import random
# A, K, Q, J, T, 9, 8, 7, 6, 5, 4, 3, or 2

# Five of a kind, where all five cards have the same label: AAAAA
# Four of a kind, where four cards have the same label and one card has a different label: AA8AA
# Full house, where three cards have the same label, and the remaining two cards share a different label: 23332
# Three of a kind, where three cards have the same label, and the remaining two cards are each different from any other card in the hand: TTT98
# Two pair, where two cards share one label, two other cards share a second label, and the remaining card has a third label: 23432
# One pair, where two cards share one label, and the other three cards have a different label from the pair and each other: A23A4
# High card, where all cards' labels are distinct: 23456

cards = ['A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2']

card_values = {card: value for card, value in zip(cards, range(1, 14))} 

class Hand():

    def __init__(self, cards):
        self.cards = cards
        # dict of card label and count
        self.counts = {card: cards.count(card) for card in cards}

    def type(self):
        if len(self.counts) == 1:
            return 1
        if len(self.counts) == 2:
            if 4 in self.counts.values():
                return 2
            else:
                return 3
        if len(self.counts) == 3:
            if 3 in self.counts.values():
                return 4
            else:
                return 5
        if len(self.counts) == 4:
            return 6
        return 7
    
    def __hash__(self) -> int:
        return self.cards.__hash__()
    
    def __eq__(self, __value: object) -> bool:
        return self.cards == __value.cards
    
    def __gt__(self, __value: object) -> bool:
        if self.type() == __value.type():
            for s, o in zip(self.cards, __value.cards):
                if card_values[s] != card_values[o]:
                    return card_values[s] > card_values[o]
        else:
            return self.type() > __value.type()
        
    def __lt__(self, __value: object) -> bool:
        return not self.__gt__(__value)
    
    def __repr__(self) -> str:
        return self.cards
    
# for i in range(10):
#     hands = [Hand('AAAAA'), Hand('22222'), Hand('AA8AA'), Hand('23332'), Hand('TTT98'), Hand('23432'), Hand('A23A4'), Hand('23456')]
#     random.shuffle(hands)
#     print(f' shuffled: |{hands}|, sorted: |{sorted(hands)}|')
    
bidmap = {}
bids = []

for line in open('input.txt', 'r'):
    hand = Hand(line.split()[0])
    bid = int(line.split()[1])
    if hand in bidmap:
        raise Exception('duplicate hand')
    bidmap[hand] = bid
    bids.append((hand, bid))

bids.sort(key=lambda x: x[0], reverse=True)
bids.insert(0, (Hand(''), 0))


sum = sum([rank * bids[rank][1] for rank in range(1, len(bids))])
print(sum)