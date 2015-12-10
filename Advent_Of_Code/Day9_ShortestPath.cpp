#include <iostream>
#include <set>
#include <map>
#include <string>

#define PART 2

using namespace std;

set<string> cities;
map<pair<string, string>, int>  edge_costs;// key: from->to pair, value is the cost

bool is_better(int current, int new_one);


class Node
{
public:
	string city;
	int depth;
	int cost_so_far;
	Node* parent;
	set<string> tried_routes;// What routes have we tried from this node and what were their costs?
	set<string> cities_visited;

	Node(const string& city, int depth, int cost_so_far, Node* parent)
		: city(city),
		depth(depth),
		cost_so_far(cost_so_far),
		parent(parent),
		tried_routes(tried_routes)
	{
		if (parent != nullptr)
			cities_visited = set<string>(parent->cities_visited);

		cities_visited.insert(city);
	}

	map<pair<string, string>, int> Available_Cities_From_This(int cost_limit)
	{
		map<pair<string, string>, int> ret;

		for (auto elem : edge_costs)
			if (elem.first.first == city &&
				tried_routes.find(elem.first.second) == tried_routes.end() && // we didnt try this path yet
#if PART == 1 
				is_better(cost_limit,cost_so_far + elem.second) && 
#endif
				cities_visited.find(elem.first.second) == cities_visited.end()) // this city has not been visited on the road so far
				ret.insert(elem);
		return ret;
	}

	bool Is_Final()
	{
		return depth == cities.size();
	}
};

bool is_better(int current, int new_one)
{
	return new_one 
#if PART == 1
				<
#else
				>
#endif
				current;
}

int Backtrack_Solver()
{
	Node* start_node = new Node("", 0, 0, nullptr);
	Node* act_node = start_node;
	int best =
#if PART == 1 
		INT_MAX;
#else
		INT_MIN;
#endif


	while (!start_node->Available_Cities_From_This(best).empty())// We try out all possibilities from the start node
	{
		map<pair<string, string>, int>  available_cities = act_node->Available_Cities_From_This(best);

		if (act_node->Is_Final() || available_cities.empty()) // backtract if we can't go any further
		{
			if (is_better(best, act_node->cost_so_far) && act_node->Is_Final())
				best = act_node->cost_so_far;

			Node* dead_end = act_node; // actual node is a dead end, so we get rid of it, and continue to find other routes from the parent
			act_node = act_node->parent;
			act_node->tried_routes.insert(dead_end->city);// We note that this route has been tried before
			delete dead_end;

			continue;
		}

		// There are some nodes to try out, we select the first
		auto edge_we_take = available_cities.begin();
		act_node = new Node(edge_we_take->first.second, act_node->depth + 1, act_node->cost_so_far + edge_we_take->second, act_node);
	}


	delete start_node;
	return best;
}


int main_day9()
{
	freopen("in.txt", "r", stdin);

	string line;
	while (getline(cin, line))
	{
		int to_pos = line.find("to");
		int eq_pos = line.find("=");

		string from_city = line.substr(0, to_pos - 1);
		string to_city = line.substr(to_pos + 3, eq_pos - 4 - to_pos);

		int cost = atoi(line.substr(eq_pos + 2).c_str());

		cities.insert(from_city);
		cities.insert(to_city);

		edge_costs[pair<string, string>(from_city, to_city)] = cost;
		edge_costs[pair<string, string>(to_city, from_city)] = cost;// The cost is symmetrical. We add it both ways for easier lookup.
	}

	for (auto city : cities)
		edge_costs[pair<string, string>("", city)] = 0;// From the start Node we can go to any city for free!

	cout << Backtrack_Solver() << endl;

	return 0;
}