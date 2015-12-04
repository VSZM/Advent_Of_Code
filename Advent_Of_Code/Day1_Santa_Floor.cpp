#include <iostream>

int main_day1()
{
	freopen("in.txt", "r", stdin);

	int floor = 0;
	char in;
	int idx = 0;
	int	basement_idx = -1;

	while (in = getchar(), ++idx, in != '\n' && in != EOF)
	{
		floor += in == '(' ? 1 : in == ')' ? -1 : 0;
		if (floor < 0 && basement_idx < 0)
			basement_idx = idx;
	}

	std::cout << "Basement idx = " << basement_idx << std::endl;
	std::cout << "final floor = " << floor << std::endl;
	return 0;
}