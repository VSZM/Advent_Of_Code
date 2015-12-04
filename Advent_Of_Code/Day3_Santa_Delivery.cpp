#include <iostream>
#include <set>

class Position
{
public:
	int x = 0;
	int y = 0;

	Position(int x, int y)
		: x(x),
		y(y)
	{

	}


	Position(const Position& other)
		: x(other.x),
		y(other.y)
	{
	}

	Position(Position&& other)
		: x(other.x),
		y(other.y)
	{
	}

	Position& operator=(const Position& other)
	{
		if (this == &other)
			return *this;
		x = other.x;
		y = other.y;
		return *this;
	}

	Position& operator=(Position&& other)
	{
		if (this == &other)
			return *this;
		x = other.x;
		y = other.y;
		return *this;
	}


	friend bool operator<(const Position& lhs, const Position& rhs)
	{
		if (lhs.x < rhs.x)
			return true;
		if (rhs.x < lhs.x)
			return false;
		return lhs.y < rhs.y;
	}

	friend bool operator<=(const Position& lhs, const Position& rhs)
	{
		return !(rhs < lhs);
	}

	friend bool operator>(const Position& lhs, const Position& rhs)
	{
		return rhs < lhs;
	}

	friend bool operator>=(const Position& lhs, const Position& rhs)
	{
		return !(lhs < rhs);
	}

	bool operator==(const Position& rhs) {
		return this->x == rhs.x && this->x == rhs.x;
	}
	bool operator!=(const Position& rhs) {
		return !(*this == rhs);
	}
};

void turn(char ch, Position& actPos)
{
	switch (ch)
	{
	case '^':
		actPos.x += 1;
		break;
	case 'v':
		actPos.x -= 1;
		break;
	case '>':
		actPos.y += 1;
		break;
	case '<':
		actPos.y -= 1;
		break;
	}
}

int main()
{
	freopen("in.txt", "r", stdin);

	Position humanPos = Position(0, 0);
	Position roboPos = Position(0, 0);

	char ch;
	bool humanTurn = true;

	std::set<Position> positions;

	positions.insert(humanPos);

	while (ch = getchar(), ch != EOF && ch != '\n')
	{
		if (humanTurn)
		{
			turn(ch, humanPos);
			positions.insert(humanPos);
		}
		else
		{
			turn(ch, roboPos);
			positions.insert(roboPos);
		}
		humanTurn = !humanTurn;
	}

	std::cout << "Number of distinct positions visited:" << positions.size() << std::endl;

	return 0;
}
