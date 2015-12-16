#include <iostream>
#include <algorithm>
#include <string>
#include <set>
#include <vector>
#include <map>
#include <boost/algorithm/string/replace.hpp>
#include <sstream>


using namespace std;

set<string> people;
// key is a pair of people: pair's first will get the value of happiness if pair's second sits next
map<pair<string, string>, int>  people_to_neighbour_happiness;


int Left_Neighbour_Happiness(int of_idx, vector<string> table)
{
	string guy = table[of_idx];
	string left_neighbour = table[of_idx == 0 ? table.size() - 1 : of_idx - 1];

	if (people_to_neighbour_happiness.count(pair<string, string>(guy, left_neighbour)) != 0)
		return people_to_neighbour_happiness[pair<string, string>(guy, left_neighbour)];

	return 0;
}

int Right_Neighbour_Happiness(int of_idx, vector<string> table)
{
	string guy = table[of_idx];
	string right_neighbour = table[of_idx == table.size() - 1 ? 0 : of_idx + 1];

	if (people_to_neighbour_happiness.count(pair<string, string>(guy, right_neighbour)) != 0)
		return people_to_neighbour_happiness[pair<string, string>(guy, right_neighbour)];

	return 0;
}

int Maximum_Happiness()
{
	int max_cost = INT_MIN;

	vector<string> combination;

	for (string guy : people)
		combination.push_back(guy);

	sort(combination.begin(), combination.end());

	do
	{
		int cost = 0;
		for (int i = 0; i < people.size(); ++i)
		{
			cost += Left_Neighbour_Happiness(i, combination);
			cost += Right_Neighbour_Happiness(i, combination);
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
		// preprocess string
		boost::replace_all(line, "would gain ", "+");
		boost::replace_all(line, "would lose ", "-");
		boost::replace_all(line, "happiness units by sitting next to ", "");
		boost::replace_all(line, ".", "");

		stringstream ss(line);

		string guy;
		ss >> guy;

		string value_str;
		ss >> value_str;
		int value = atoi(value_str.c_str());

		string neighbour;
		ss >> neighbour;

		people.insert(guy);
		people.insert(neighbour);

		people_to_neighbour_happiness[pair<string, string>(guy, neighbour)] = value;
	}

//	cout << Maximum_Happiness() << endl;
	people.insert("me");
	cout << Maximum_Happiness() << endl;


	return 0;
}