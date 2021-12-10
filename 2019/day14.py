from collections import namedtuple
from os import name

STARTING_ORE = 1000000000000

Resource = namedtuple('Resource', ['name', 'quantity'])

resource_amounts = {'ORE': STARTING_ORE}
production_map = {}

for line in open('input.txt'):
    ingredients, outcome = tuple(line.split('=>'))
    ingredients = [Resource(ingredient.split()[1], int(
        ingredient.split()[0])) for ingredient in ingredients.split(',')]
    outcome = Resource(outcome.split()[1], int(outcome.split()[0]))
    production_map[outcome.name] = {
        'ingredients': ingredients, 'outcome': outcome}
    resource_amounts[outcome.name] = 0


def stop_function_1(request_stack):
    return len(request_stack) > 0


def stop_function_2(request_stack):
    return len(request_stack) == 0 or request_stack[-1].name != 'ORE'


def value_function_1(resource_amounts):
    return STARTING_ORE - resource_amounts['ORE']


def value_function_2(resource_amounts):
    return resource_amounts['FUEL']


def solve(stop_function, value_function, request_stack=[Resource('FUEL', 1)]):

    while stop_function(request_stack):
        if len(request_stack) == 0:
            request_stack.append(Resource('FUEL', 1))
        requested = request_stack[-1]
        ingredients = production_map[requested.name]['ingredients']
        # Check if we have all ingredients. If so, produce the resource
        if all([resource_amounts[ingredient.name] >= ingredient.quantity for ingredient in ingredients]):
            outcome = production_map[requested.name]['outcome']
            # Delete all request of the same kind
            request_stack = [
                request for request in request_stack if request.name != requested.name]
            for ingredient in ingredients:
                resource_amounts[ingredient.name] -= ingredient.quantity
            resource_amounts[outcome.name] += outcome.quantity
        else:  # In case we have no ingredient, we request it
            for ingredient in ingredients:
                if resource_amounts[ingredient.name] < ingredient.quantity:
                    request_stack.append(Resource(ingredient.name, ingredient.quantity - resource_amounts[ingredient.name]))

    return value_function(resource_amounts)


print(f"Part 1 Solution: {solve(stop_function_1, value_function_1)}")
resource_amounts = {k: 0 for k in resource_amounts.keys()}
amount_of_fuel_without_remainders = STARTING_ORE / \
    (STARTING_ORE - resource_amounts['ORE'])
resource_amounts['ORE'] = STARTING_ORE
print(
    f"Part 2 Solution: {solve(stop_function_2, value_function_2, request_stack = [Resource('FUEL', amount_of_fuel_without_remainders)])}")
