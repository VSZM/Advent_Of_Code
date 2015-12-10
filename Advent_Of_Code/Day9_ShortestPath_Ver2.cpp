#include <iostream>
#include <set>
#include <map>
#include <string>
#include <vector>
#include <algorithm>

using namespace std;

set<string> villages;
map<pair<string, string>, int>  road_costs;// key: from->to pair, value is the cost


int Minimum_Cost()
{
	int min_cost = INT_MAX;

	vector<string> combination;

	for (string city : villages)
		combination.push_back(city);

	sort(combination.begin(), combination.end());

	do
	{
		int cost = 0;
		int i;
		for (i = 0; i < villages.size() -1 && cost < min_cost; ++i)// We are checking the current permutation until we are sure it can't be better than the minimal
		{
			cost += road_costs[pair<string, string>(combination[i], combination[i + 1])];
		}

		if (i == villages.size() - 1 && cost < min_cost)
		{
			min_cost = cost;
		}
	} while (next_permutation(combination.begin(), combination.end()));// We are generating all the permutations of the cities

	return min_cost;
}

int Maximum_Cost()
{
	int max_cost = INT_MIN;

	vector<string> combination;

	for (string city : villages)
		combination.push_back(city);

	sort(combination.begin(), combination.end());

	do
	{
		int cost = 0;
		for (int i = 0; i < villages.size() - 1; ++i)
		{
			cost += road_costs[pair<string, string>(combination[i], combination[i + 1])];
		}

		if (cost > max_cost)
		{
			max_cost = cost;
		}
	} while (next_permutation(combination.begin(), combination.end()));

	return max_cost;
}

int main()
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

		villages.insert(from_city);
		villages.insert(to_city);

		road_costs[pair<string, string>(from_city, to_city)] = cost;
		road_costs[pair<string, string>(to_city, from_city)] = cost;// The cost is symmetrical. We add it both ways for easier lookup.
	}


	cout << Minimum_Cost() << endl;
	cout << Maximum_Cost() << endl;

	return 0;
}