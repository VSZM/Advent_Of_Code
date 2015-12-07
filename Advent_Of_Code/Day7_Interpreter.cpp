#include <iostream>
#include <string>
#include <map>
#include <atomic>

std::map<std::string, std::string> registerExpressionMap;

std::map<std::string, int> cache;

uint16_t evaluate(std::string expression)
{
	if (cache.find(expression) != cache.end())// cache hit
		return cache[expression];

	if (expression.find("AND") != std::string::npos)
	{
		size_t pos = expression.find_first_of("AND");

		int left = evaluate(expression.substr(0, pos - 1));
		int right = evaluate(expression.substr(pos + 4));

		cache[expression] = left & right;
		return cache[expression];
	}
	if (expression.find("OR") != std::string::npos)
	{
		size_t pos = expression.find_first_of("OR");

		int left = evaluate(expression.substr(0, pos - 1));
		int right = evaluate(expression.substr(pos + 3));

		cache[expression] = left | right;
		return cache[expression];
	}
	if (expression.find("NOT") != std::string::npos)
	{
		cache[expression] = ~evaluate(expression.substr(4));
		return cache[expression];
	}
	if (expression.find("LSHIFT") != std::string::npos)
	{
		size_t pos = expression.find_first_of("LSHIFT");

		int left = evaluate(expression.substr(0, pos - 1));
		int right = evaluate(expression.substr(pos + 7));

		cache[expression] = left << right;
		return  cache[expression];
	}
	if (expression.find("RSHIFT") != std::string::npos)
	{
		size_t pos = expression.find_first_of("RSHIFT");

		int left = evaluate(expression.substr(0, pos - 1));
		int right = evaluate(expression.substr(pos + 7));

		cache[expression] = left >> right;
		return cache[expression];
	}
	if (isdigit(expression[0])) // Direct number assignment
	{
		cache[expression] = atoi(expression.c_str());
		return cache[expression];
	}
	if (registerExpressionMap.find(expression) != registerExpressionMap.end()){	// A register
		cache[expression] = evaluate(registerExpressionMap[expression]);
		return cache[expression];
	}

	throw "Unexpected expression: " + expression;
}

int main()
{
	freopen("in.txt", "r", stdin);
	freopen("out.txt", "w", stdout);

	std::string line;
	do
	{
		getline(std::cin, line);
		size_t pos = line.find_first_of("->");
		registerExpressionMap[line.substr(pos + 3)] = line.substr(0, pos - 1);
	} while (!std::cin.eof());

	int a = evaluate("a");

	std::cout << evaluate("a") << std::endl;

	// PART2

	cache.erase(cache.begin(),cache.end());

	registerExpressionMap["b"] = std::to_string(a);

	std::cout << evaluate("a") << std::endl;

	return 0;
}