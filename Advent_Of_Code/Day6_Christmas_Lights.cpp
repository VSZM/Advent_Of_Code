#include <iostream>
#include <string>

enum Action{ON,OFF,TOGGLE};


int grid[1000][1000] = { 0 };

class Line
{
public:
	int fromX;
	int fromY;
	int toX;
	int toY;
	Action action;


	Line(int from_x, int from_y, int to_x, int to_y, Action action)
		: fromX(from_x),
		  fromY(from_y),
		  toX(to_x),
		  toY(to_y),
		  action(action)
	{
	}


};

Line* read_line()
{
	std::string tmp;

	std::cin >> tmp;

	Action action;

	if (tmp == "turn")
	{
		std::cin >> tmp;
		if (tmp == "on")
		{
			action = ON;
		} else
		{
			action = OFF;
		}
	} else if (tmp == "toggle")
	{
		action = TOGGLE;
	} else
	{
		return nullptr;
	}

	int from_x, from_y, to_x, to_y;

	scanf("%d, %d through %d, %d", &from_x, &from_y, &to_x, &to_y);

	return new Line(from_x, from_y, to_x, to_y, action);
}

int on_count()// part 1
{
	int count = 0;

	for (int i = 0; i < 1000; ++i)
		for (int j = 0; j < 1000; ++j)
			count += grid[i][j] == 0 ? 0 : 1;

	return count;
}

int total_brightness()// part2
{
	int sum = 0;

	for (int i = 0; i < 1000; ++i)
		for (int j = 0; j < 1000; ++j)
			sum += grid[i][j];

	return sum;
}

void interpret_action_part1(int i, int j, Action action)
{
	switch (action)
	{
	case ON:
		grid[i][j] = 1;
		break;
	case OFF:
		grid[i][j] = 0;
		break;
	case TOGGLE:
		grid[i][j] = !grid[i][j];
		break;
	}
}

void interpret_action_part2(int i, int j, Action action)
{
	switch (action)
	{
	case ON:
		grid[i][j] += 1;
		break;
	case OFF:
		if (grid[i][j])
			grid[i][j] -= 1;
		break;
	case TOGGLE:
		grid[i][j] += 2;
		break;
	}
}

int main()
{
	freopen("in.txt", "r", stdin);

	Line* actline;

	while (actline = read_line(), nullptr != actline)
	{
		for (int i = actline->fromX; i <= actline->toX; ++i)
		{
			for (int j = actline->fromY; j <= actline->toY; ++j)
			{
				interpret_action_part2(i, j, actline->action);
			}
		}
	}

	std::cout << total_brightness() << std::endl;

	return 0;
}