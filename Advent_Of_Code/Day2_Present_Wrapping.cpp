#include <iostream>
#include <algorithm>

int main_day2()
{
	freopen("in.txt", "r", stdin);

	int a, b, c;
	int sum_paper = 0;
	int sum_ribbon = 0;

	while (scanf("%dx%dx%d", &a, &b, &c) > 0)
	{
		int A1 = a*b, A2 = b*c, A3 = a*c;
		int P1 = 2*(a+b), P2 = 2*(b+c), P3 = 2*(a+c);
		int vol = a*b*c;

		sum_paper += 2 * A1 + 2 * A2 + 2 * A3 + std::min({ A1, A2, A3 });
		sum_ribbon += vol + std::min({ P1, P2, P3 });
	}

	std::cout << "Wrapping paper required: " << sum_paper << std::endl;
	std::cout << "Ribbon required: " << sum_ribbon << std::endl;

	return 0;
}