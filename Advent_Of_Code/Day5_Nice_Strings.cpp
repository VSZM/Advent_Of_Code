#include <iostream>
#include <string>
#include <vector>

std::vector<char> vowels = { 'a', 'e', 'i', 'o', 'u' };
std::string banned_words[] = { std::string("ab"), std::string("cd"), std::string("pq"), std::string("xy") };


bool is_nice_part1(std::string word)
{
	bool contains_double = false;
	int vowel_count = find(begin(vowels), end(vowels), word[0]) == vowels.end() ? 0 : 1;// check if first elem is vowel

	for (int i = 1; i < word.length(); ++i)
	{
		for (auto bword : banned_words)
		{
			if (bword[0] == word[i - 1] && bword[1] == word[i])
				return false;
		}

		if (word[i] == word[i - 1])
			contains_double = true;

		vowel_count += find(begin(vowels), end(vowels), word[i]) == vowels.end() ? 0 : 1;
	}

	return contains_double && vowel_count >= 3;
}

bool is_nice_part2(std::string word)
{
	bool contains_pair_twice = false;
	bool contains_repetition = false;

	for (int i = 2; i < word.length(); ++i)
	{
		if (word[i - 2] == word[i])
			contains_repetition = true;

		for (int j = i + 1; j < word.length() && !contains_pair_twice; ++j)
		{
			if (word[i - 2] == word[j - 1] && word[i - 1] == word[j])
				contains_pair_twice = true;
		}

	}
	return contains_pair_twice && contains_repetition;
}

int main()
{
	freopen("in.txt", "r", stdin);

	std::string word;
	int nice_count = 0;

	while (std::cin >> word)
	{
		if (is_nice_part2(word))
			++nice_count;
	}

	std::cout << nice_count << std::endl;

	return 0;
}