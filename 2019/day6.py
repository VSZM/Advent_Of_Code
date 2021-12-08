

def calculateRoute(parent_dict, child):
    if child in parent_dict:
        return 1 + calculateRoute(parent_dict, parent_dict[child])

    return 0

def calculateAllRoutes(parent_dict):
    sum = 0

    for node in parent_dict.keys():
        sum += calculateRoute(parent_dict, node)

    return sum

def getAncestors(parent_dict, child):

    ancestorList = []

    current = parent_dict[child]
    while current in parent_dict:
        ancestorList.append(current)
        current = parent_dict[current]
    
    return ancestorList

def getRouteBetween(parent_dict, child1, child2):
    ancestorList1 = getAncestors(parent_dict, child1)
    ancestorList2 = getAncestors(parent_dict, child2)

    return len(set(ancestorList1).symmetric_difference(set(ancestorList2)))

if __name__ == "__main__":
    with open('day6.txt', 'r') as f:
        lines = f.readlines()
        parent_dict = {}
        for line in lines:
            parent, child = line.strip().split(')')
            parent_dict[child] = parent
            
    print(calculateAllRoutes(parent_dict))
    print(getRouteBetween(parent_dict, 'YOU', 'SAN'))

