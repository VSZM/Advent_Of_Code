#include <iostream>
#include <string>

using namespace std;

string say_numbers(string str)
{
	string ret;

	for (int i = 0; i < str.size(); ++i)
	{
		int j;
		for (j = i; str[i] == str[j]; ++j);// We go until we find a different number
		ret += to_string(j - i) + str[i]; // We add the count of this kind and the kind itself
		i = j-1;
	}

	return ret;
}

int main()
{
	string number = "1113222113";


	for (int i = 0; i < 50; ++i)
	{
		number = say_numbers(number);
	}

	//cout << number << endl;
	cout << number.size() << endl;

	return 0;
}