#include <string>
#include <iostream>

int get_memory_length(std::string word)
{
	int length = word.length() - 2;

	for (int i = 1; i < word.length() - 1; ++i)
	{
		if (word[i] == '\\')
		{
			if (word[i + 1] == 'x')
			{
				i += 3;
				length -= 3;
			}
			else
			{
				++i;
				--length;
			}
		}

	}


	return length;
}

int get_representation_length(std::string word)// PART 2
{
	int length = 2;

	for (int i = 0; i < word.length(); ++i)
		if (word[i] == '\\' || word[i] == '"')
			++length;

	return length + word.length();
}

int main()
{
	freopen("in.txt", "r", stdin);

	std::string word;
	int sum_code_length = 0;
	int sum_memory_length = 0;
	int sum_representation_length = 0;


	while (std::cin >> word)
	{
		sum_code_length += word.length();
		sum_memory_length += get_memory_length(word);
		sum_representation_length += get_representation_length(word);
	}

	std::cout << sum_code_length - sum_memory_length << std::endl;

	// PART 2

	std::cout << sum_representation_length - sum_code_length << std::endl;

	return 0;
}